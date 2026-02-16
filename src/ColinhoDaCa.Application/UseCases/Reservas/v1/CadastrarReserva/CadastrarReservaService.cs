using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.Services.EmailTemplates;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

public class CadastrarReservaService : ICadastrarReservaService
{
    private readonly ILogger<CadastrarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPetRepository _petRepository;
    private readonly IRacaRepository _racaRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public CadastrarReservaService(ILogger<CadastrarReservaService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository,
        IClienteRepository clienteRepository,
        IUsuarioRepository usuarioRepository,
        IPetRepository petRepository,
        IRacaRepository racaRepository,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
        _clienteRepository = clienteRepository;
        _usuarioRepository = usuarioRepository;
        _petRepository = petRepository;
        _racaRepository = racaRepository;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task Handle(CadastrarReservaCommand command)
    {
        try
        {
            var reserva = ReservaDb.Create(command.ClienteId, command.DataInicial, command.DataFinal, command.QuantidadeDiarias, command.QuantidadePets, command.ValorTotal, command.ValorDesconto, command.ValorFinal, command.CupomId, command.Observacoes, command.PetIds, command.UsuarioId);

            await _reservaRepository.InsertAsync(reserva);
            await _unitOfWork.CommitAsync();

            var usuarioDb = await _usuarioRepository.GetAsync(u => u.ClienteId == command.ClienteId);
            if (usuarioDb != null)
            {
                var statusHistorico = ReservaStatusHistoricoDb.Create(reserva.Id, ReservaStatus.ReservaCriada, usuarioDb.Id);
                reserva.AdicionarStatusHistorico(statusHistorico);
                await _unitOfWork.CommitAsync();
            }

            var cliente = await _clienteRepository.GetAsync(c => c.Id == command.ClienteId);
            if (cliente != null)
            {
            // Otimizar consultas - buscar todos os pets e raças de uma vez
            var petsQuery = await _petRepository.GetAllAsync();
            var petsReserva = petsQuery.Where(p => command.PetIds.Contains(p.Id)).ToList();
            
            var racaIds = petsReserva.Where(p => p.RacaId.HasValue).Select(p => p.RacaId.Value).Distinct().ToList();
            var racasQuery = await _racaRepository.GetAllAsync();
            var racas = racasQuery.Where(r => racaIds.Contains(r.Id)).ToDictionary(r => r.Id, r => r.Nome);

                var pets = petsReserva.Select(pet => (
                    Nome: pet.Nome,
                    Raca: pet.RacaId.HasValue && racas.ContainsKey(pet.RacaId.Value) 
                        ? racas[pet.RacaId.Value] 
                        : "Sem raça"
                )).ToList();

                var assunto = $"Nova Reserva Criada #{reserva.Id.ToString().PadLeft(6, '0')} - Colinho da Cá";
                var corpo = EmailTemplateService.GerarEmailNovaReserva(
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
            _logger.LogError(ex, "Problema na Inclusão de Reservas");
            throw;
        }
    }
}