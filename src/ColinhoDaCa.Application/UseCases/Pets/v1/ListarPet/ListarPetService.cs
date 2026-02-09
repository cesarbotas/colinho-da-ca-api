using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;
using ColinhoDaCa.Domain.Pets.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;

public class ListarPetService : IListarPetService
{
    private readonly ILogger<ListarPetService> _logger;
    private readonly IPetReadRepository _petReadRepository;

    public ListarPetService(ILogger<ListarPetService> logger, 
        IPetReadRepository petReadRepository)
    {
        _logger = logger;
        _petReadRepository = petReadRepository;
    }

    public async Task<ResultadoPaginadoDto<PetsDto>> Handle(ListarPetQuery query)
    {
        try
        {
            return await _petReadRepository.PesquisarPetsDto(query);
        }
        catch (Exception)
        {
            throw;
        }
    }
}