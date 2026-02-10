using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Reservas;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using ColinhoDaCa.Domain.Reservas.Entities;

namespace ColinhoDaCa.Domain.Reservas.Repositories;

public interface IReservaReadRepository
{
    IQueryable<ReservaDb> AsQueryable();
    Task<ResultadoPaginadoDto<ReservasDto>> PesquisarReservasDto(ListarReservaQuery query);
}
