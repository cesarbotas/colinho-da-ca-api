using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Reservas;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;

public class ListarReservaService : IListarReservaService
{
    private readonly ILogger<ListarReservaService> _logger;
    private readonly IReservaReadRepository _reservaReadRepository;

    public ListarReservaService(ILogger<ListarReservaService> logger, 
        IReservaReadRepository reservaReadRepository)
    {
        _logger = logger;
        _reservaReadRepository = reservaReadRepository;
    }

    public async Task<ResultadoPaginadoDto<ReservasDto>> Handle(ListarReservaQuery query)
    {
        try
        {
            return await _reservaReadRepository.PesquisarReservasDto(query);
        }
        catch (Exception)
        {
            throw;
        }
    }
}