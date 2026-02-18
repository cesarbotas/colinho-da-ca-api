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

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.EnviarComprovante;

public class EnviarComprovanteService : IEnviarComprovanteService
{
    private readonly ILogger<EnviarComprovanteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPetRepository _petRepository;
    private readonly IRacaRepository _racaRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public EnviarComprovanteService(
        ILogger<EnviarComprovanteService> logger,
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

    public async Task Handle(long reservaId, EnviarComprovanteCommand command)
    {
        try
        {
            var reserva = await _reservaRepository.GetAsync(r => r.Id == reservaId);

            if (reserva == null)
            {
                throw new EntityNotFoundException("Reserva não encontrada");
            }

            if (reserva.Status != ReservaStatus.PagamentoPendente)
            {
                throw new ValidationException("Somente reservas com status 'Pagamento Pendente' podem receber comprovante");
            }

            reserva.EnviarComprovantePagamento(command.ComprovantePagamento, command.ObservacoesPagamento);

            _reservaRepository.Update(reserva);
            await _unitOfWork.CommitAsync();

            var cliente = await _clienteRepository.GetAsync(c => c.Id == reserva.ClienteId);
            if (cliente != null)
            {
                var assunto = $"Comprovante Recebido #{reserva.Id.ToString().PadLeft(6, '0')} - Colinho da Cá";
                var corpo = EmailTemplateService.GerarEmailComprovanteRecebido(
                    cliente.Nome,
                    reserva.Id,
                    reserva.DataInicial,
                    reserva.DataFinal,
                    reserva.QuantidadeDiarias,
                    reserva.QuantidadePets,
                    reserva.ValorTotal,
                    reserva.ValorDesconto,
                    reserva.ValorFinal,
                    command.ObservacoesPagamento,
                    new List<(string Nome, string Raca)>());
                var emailDestino = _configuration["Email:EmailDestino"];
                await _emailService.EnviarEmailAsync(emailDestino, assunto, corpo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao enviar comprovante");
            throw;
        }
    }
}
