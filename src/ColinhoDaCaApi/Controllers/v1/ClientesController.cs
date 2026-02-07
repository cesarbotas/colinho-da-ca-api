using ColinhoDaCa.Application.UseCases.Clientes.CadastrarCliente;
using ColinhoDaCa.Domain.Clientes.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
//[Authorize]
[Route("api/v1/clientes")]
[ApiExplorerSettings(GroupName = "v1")]
public class ClientesController : Controller
{
    private readonly ILogger<ClientesController> _logger;
    private readonly ICadastrarClienteService _cadastrarClienteService;

    public ClientesController(ILogger<ClientesController> logger, 
        ICadastrarClienteService cadastrarClienteService)
    {
        _logger = logger;
        _cadastrarClienteService = cadastrarClienteService;
    }

    [HttpPost("", Name = "")]
    public async Task<ActionResult> CadastrarCliente([FromBody] CadastrarClienteCommand command)
    {
        await _cadastrarClienteService.Execute(command);

        return Ok();
    }
}
