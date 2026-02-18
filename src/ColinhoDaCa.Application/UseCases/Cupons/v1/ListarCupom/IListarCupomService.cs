using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Cupons;

namespace ColinhoDaCa.Application.UseCases.Cupons.v1.ListarCupom;

public interface IListarCupomService
{
    Task<ResultadoPaginadoDto<CupomDto>> Handle(ListarCupomQuery query);
}
