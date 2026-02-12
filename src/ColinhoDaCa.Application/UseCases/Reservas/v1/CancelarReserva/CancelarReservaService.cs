using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CancelarReserva;

public class CancelarReservaService : ICancelarReservaService
{
    private readonly ILogger<CancelarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CancelarReservaService(
        ILogger<CancelarReservaService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
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

            var usuarioId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            reserva.CancelarReserva(usuarioId);

            _reservaRepository.Update(reserva);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao cancelar reserva");
            throw;
        }
    }
}
