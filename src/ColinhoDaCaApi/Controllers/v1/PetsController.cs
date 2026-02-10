using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;
using ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Route("api/v1/pets")]
[ApiExplorerSettings(GroupName = "v1")]
public class PetsController : Controller
{
    private readonly ILogger<PetsController> _logger;
    private readonly ICadastrarPetService _cadastrarPetService;
    private readonly IListarPetService _listarPetService;
    private readonly IAlterarPetService _alterarPetService;
    private readonly IExcluirPetService _excluirPetService;

    public PetsController(ILogger<PetsController> logger,
        ICadastrarPetService cadastrarPetService,
        IListarPetService listarPetService,
        IAlterarPetService alterarPetService,
        IExcluirPetService excluirPetService)
    {
        _logger = logger;
        _cadastrarPetService = cadastrarPetService;
        _listarPetService = listarPetService;
        _alterarPetService = alterarPetService;
        _excluirPetService = excluirPetService;
    }

    [HttpGet("", Name = "ListarPets")]
    public async Task<ActionResult<ResultadoPaginadoDto<PetsDto>>> ListarPets([FromQuery] ListarPetQuery query)
    {
        var result = await _listarPetService.Handle(query);

        return Ok(result);
    }

    [HttpPost("", Name = "IncluirPet")]
    public async Task<ActionResult> CadastrarPet([FromBody] CadastrarPetCommand command)
    {
        await _cadastrarPetService.Handle(command);

        return Ok();
    }

    [HttpPut("{id}", Name = "AlterarPet")]
    public async Task<ActionResult> AlterarPet([FromRoute] long id, [FromBody] AlterarPetCommand command)
    {
        await _alterarPetService.Handle(id, command);

        return Ok();
    }

    [HttpDelete("{id}", Name = "ExcluirPet")]
    public async Task<ActionResult> ExcluirPet([FromRoute] long id)
    {
        await _excluirPetService.Handle(id);

        return Ok();
    }
}