using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;

public interface IListarPetService
{
    Task<ResultadoPaginadoDto<PetsDto>> Handle(ListarPetQuery query);
}
