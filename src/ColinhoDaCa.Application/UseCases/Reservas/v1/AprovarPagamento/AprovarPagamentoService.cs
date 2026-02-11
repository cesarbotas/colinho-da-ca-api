using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AprovarPagamento;

public class AprovarPagamentoService : IAprovarPagamentoService
{
    private readonly ILogger<AprovarPagamentoService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AprovarPagamentoService(
        ILogger<AprovarPagamentoService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository,
        IClienteRepository clienteRepository,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
        _clienteRepository = clienteRepository;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(long reservaId)
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
                throw new ValidationException("Somente reservas com status 'Pagamento Pendente' podem ter pagamento aprovado");
            }

            if (string.IsNullOrEmpty(reserva.ComprovantePagamento))
            {
                throw new ValidationException("Comprovante de pagamento não foi enviado");
            }

            var usuarioId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            reserva.AprovarPagamento(usuarioId);
            reserva.FinalizarReserva(usuarioId);

            _reservaRepository.Update(reserva);
            await _unitOfWork.CommitAsync();

            var cliente = await _clienteRepository.GetAsync(c => c.Id == reserva.ClienteId);

            await _emailService.EnviarEmailAsync(
                cliente.Email,
                "Pagamento Aprovado - Reserva Confirmada - Colinho da Cá",
                $@"Olá {cliente.Nome},

Seu pagamento foi aprovado e sua reserva está confirmada!

Detalhes da Reserva:
- Período: {reserva.DataInicial:dd/MM/yyyy} a {reserva.DataFinal:dd/MM/yyyy}
- Quantidade de Diárias: {reserva.QuantidadeDiarias}
- Quantidade de Pets: {reserva.QuantidadePets}
- Valor Total: R$ {reserva.ValorTotal:N2}

Status: Reserva Finalizada

Aguardamos você e seus pets!

Atenciosamente,
Equipe Colinho da Cá"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao aprovar pagamento");
            throw;
        }
    }
}
