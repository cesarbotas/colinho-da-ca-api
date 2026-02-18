using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ConcederDesconto;

public class ConcederDescontoService : IConcederDescontoService
{
    private readonly ILogger<ConcederDescontoService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;

    public ConcederDescontoService(
        ILogger<ConcederDescontoService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
    }

    public async Task Handle(long reservaId, ConcederDescontoCommand command)
    {
        try
        {
            var reserva = await _reservaRepository.GetAsync(r => r.Id == reservaId);

            if (reserva == null)
            {
                throw new EntityNotFoundException("Reserva não encontrada");
            }

            if (reserva.Status != ReservaStatus.ReservaCriada)
            {
                throw new ValidationException("Desconto só pode ser concedido em reservas com status 'Reserva Criada'");
            }

            if (command.ValorDesconto < 0)
            {
                throw new ValidationException("Valor do desconto não pode ser negativo");
            }

            if (command.ValorDesconto > reserva.ValorTotal)
            {
                throw new ValidationException("Valor do desconto não pode ser maior que o valor total");
            }

            reserva.ConcederDesconto(command.ValorDesconto);

            _reservaRepository.Update(reserva);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao conceder desconto");
            throw;
        }
    }
}
