using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ExcluirReserva;

public class ExcluirReservaService : IExcluirReservaService
{
    private readonly ILogger<ExcluirReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;

    public ExcluirReservaService(ILogger<ExcluirReservaService> logger, 
        IUnitOfWork unitOfWork, 
        IReservaRepository reservaRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
    }

    public async Task Handle(long id)
    {
        try
        {
            var reserva = await _reservaRepository.GetAsync(id);

            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada");
            }

            _reservaRepository.Delete(reserva);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Exclusão de Reservas");

            throw;
        }
    }
}