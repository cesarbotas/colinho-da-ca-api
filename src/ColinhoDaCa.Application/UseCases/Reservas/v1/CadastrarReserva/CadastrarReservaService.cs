using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

public class CadastrarReservaService : ICadastrarReservaService
{
    private readonly ILogger<CadastrarReservaService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservaRepository _reservaRepository;

    public CadastrarReservaService(ILogger<CadastrarReservaService> logger,
        IUnitOfWork unitOfWork,
        IReservaRepository reservaRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _reservaRepository = reservaRepository;
    }

    public async Task Handle(CadastrarReservaCommand command)
    {
        try
        {
            var reserva = ReservaDb.Create(command.ClienteId, command.DataInicial, command.DataFinal, command.QuantidadeDiarias, command.QuantidadePets, command.ValorTotal, command.Observacoes, command.PetIds, command.UsuarioId);

            await _reservaRepository.InsertAsync(reserva);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Inclusão de Reservas");

            throw;
        }
    }
}