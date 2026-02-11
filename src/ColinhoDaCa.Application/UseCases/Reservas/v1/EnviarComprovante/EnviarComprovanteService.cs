using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.EnviarComprovante;

public class EnviarComprovanteService : IEnviarComprovanteService
{
    private readonly ILogger<EnviarComprovanteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;

    public EnviarComprovanteService(
        ILogger<EnviarComprovanteService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao enviar comprovante");
            throw;
        }
    }
}
