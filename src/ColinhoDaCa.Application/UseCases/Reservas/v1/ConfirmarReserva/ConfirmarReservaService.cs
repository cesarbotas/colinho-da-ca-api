using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.Services.EmailTemplates;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ConfirmarReserva;

public class ConfirmarReservaService : IConfirmarReservaService
{
    private readonly ILogger<ConfirmarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPetRepository _petRepository;
    private readonly IRacaRepository _racaRepository;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConfirmarReservaService(
        ILogger<ConfirmarReservaService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository,
        IClienteRepository clienteRepository,
        IPetRepository petRepository,
        IRacaRepository racaRepository,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
        _clienteRepository = clienteRepository;
        _petRepository = petRepository;
        _racaRepository = racaRepository;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(long reservaId)
    {
        try
        {
            var reserva = await _reservaRepository.GetWithRelationsAsync(reservaId);

            if (reserva == null)
            {
                throw new EntityNotFoundException("Reserva não encontrada");
            }

            if (reserva.Status != ReservaStatus.ReservaCriada)
            {
                throw new ValidationException("Somente reservas com status 'Reserva Criada' podem ser confirmadas");
            }

            var usuarioId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            reserva.ConfirmarReserva(usuarioId);
            reserva.AlterarParaPagamentoPendente(usuarioId);

            _reservaRepository.Update(reserva);
            await _unitOfWork.SaveChangesAsync();

            var cliente = await _clienteRepository.GetAsync(c => c.Id == reserva.ClienteId);

            var assunto = $"Reserva Confirmada #{reserva.Id.ToString().PadLeft(6, '0')} - Colinho da Cá";
            var corpoHtml = EmailTemplateService.GerarEmailConfirmacao(
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
                new List<(string Nome, string Raca)>()
            );
            await _emailService.EnviarEmailAsync(cliente.Email, assunto, corpoHtml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao confirmar reserva");
            throw;
        }
    }
}
