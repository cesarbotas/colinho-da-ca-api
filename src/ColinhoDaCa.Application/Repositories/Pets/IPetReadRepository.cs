using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using ColinhoDaCa.Domain.Pets.Entities;

namespace ColinhoDaCa.Domain.Pets.Repositories;

public interface IPetReadRepository
{
    IQueryable<PetDb> AsQueryable();
    Task<ResultadoPaginadoDto<PetsDto>> PesquisarPetsDto(ListarPetQuery query);
}
