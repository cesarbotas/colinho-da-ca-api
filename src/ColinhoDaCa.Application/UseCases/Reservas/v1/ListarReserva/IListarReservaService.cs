using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Reservas;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;

public interface IListarReservaService
{
    Task<ResultadoPaginadoDto<ReservasDto>> Handle(ListarReservaQuery query);
}