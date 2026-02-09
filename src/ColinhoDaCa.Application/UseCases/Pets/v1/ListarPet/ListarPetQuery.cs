using ColinhoDaCa.Application._Shared.DTOs.Paginacao;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;

public class ListarPetQuery
{
    public long? ClienteId { get; set; }
    public PaginacaoDto Paginacao { get; set; }
}