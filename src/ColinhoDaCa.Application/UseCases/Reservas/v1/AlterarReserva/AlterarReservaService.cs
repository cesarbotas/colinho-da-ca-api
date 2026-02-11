using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;

public class AlterarReservaService : IAlterarReservaService
{
    private readonly ILogger<AlterarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;

    public AlterarReservaService(ILogger<AlterarReservaService> logger, 
        IUnitOfWork unitOfWork, 
        IReservaRepository reservaRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
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

             reserva.Alterar(command.ClienteId, command.DataInicial, command.DataFinal, command.QuantidadeDiarias, command.QuantidadePets, command.ValorTotal, command.Observacoes, command.PetIds);

             _reservaRepository.Update(reserva);
            
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Alteração de Reservas");

            throw;
        }
    }
}