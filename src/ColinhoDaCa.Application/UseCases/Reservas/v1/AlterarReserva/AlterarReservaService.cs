using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.Services.EmailTemplates;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;

public class AlterarReservaService : IAlterarReservaService
{
    private readonly ILogger<AlterarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPetRepository _petRepository;
    private readonly IRacaRepository _racaRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AlterarReservaService(ILogger<AlterarReservaService> logger, 
        IUnitOfWork unitOfWork, 
        IReservaRepository reservaRepository,
        IClienteRepository clienteRepository,
        IPetRepository petRepository,
        IRacaRepository racaRepository,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
        _clienteRepository = clienteRepository;
        _petRepository = petRepository;
        _racaRepository = racaRepository;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task Handle(long id, AlterarReservaCommand command)
    {
        try
        {
            var reserva = await _reservaRepository.GetAsync(c => c.Id == id);

            if (reserva == null)
            {
                throw new EntityNotFoundException("Reserva não encontrada");
            }

            if (reserva.Status != ReservaStatus.ReservaCriada)
            {
                throw new ValidationException("Somente reservas com status 'Reserva Criada' podem ser alteradas pelo cliente");
            }

             reserva.Alterar(command.ClienteId, command.DataInicial, command.DataFinal, command.QuantidadeDiarias, command.QuantidadePets, command.ValorTotal, command.ValorDesconto, command.ValorFinal, command.CupomId, command.Observacoes, command.PetIds);

             _reservaRepository.Update(reserva);
            await _unitOfWork.CommitAsync();

            var cliente = await _clienteRepository.GetAsync(c => c.Id == command.ClienteId);
            if (cliente != null)
            {
                var pets = new List<(string Nome, string Raca)>();
                foreach (var petId in command.PetIds)
                {
                    var pet = await _petRepository.GetAsync(p => p.Id == petId);
                    if (pet != null)
                    {
                        var racaNome = "Sem raça";
                        if (pet.RacaId.HasValue)
                        {
                            var raca = await _racaRepository.GetByIdAsync(pet.RacaId.Value);
                            if (raca != null) racaNome = raca.Nome;
                        }
                        pets.Add((pet.Nome, racaNome));
                    }
                }

                var assunto = $"Reserva Alterada #{reserva.Id.ToString().PadLeft(6, '0')} - Colinho da Cá";
                var corpo = EmailTemplateService.GerarEmailReservaAlterada(
                    cliente.Nome,
                    reserva.Id,
                    reserva.DataInicial,
                    reserva.DataFinal,
                    reserva.QuantidadeDiarias,
                    reserva.QuantidadePets,
                    reserva.ValorTotal,
                    reserva.ValorDesconto,
                    reserva.ValorFinal,
                    reserva.Observacoes,
                    pets);
                var emailDestino = _configuration["Email:EmailDestino"];
                await _emailService.EnviarEmailAsync(emailDestino, assunto, corpo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Alteração de Reservas");
            throw;
        }
    }
}