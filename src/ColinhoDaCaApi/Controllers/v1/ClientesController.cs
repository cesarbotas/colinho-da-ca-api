using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
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
    private readonly IListarClienteService _listarClienteService;

    public ClientesController(ILogger<ClientesController> logger,
        ICadastrarClienteService cadastrarClienteService,
        IListarClienteService listarClienteService)
    {
        _logger = logger;
        _cadastrarClienteService = cadastrarClienteService;
        _listarClienteService = listarClienteService;
    }

    [HttpGet("", Name = "")]
    public async Task<ActionResult<IEnumerable<ClienteDb>>> ListarClientes([FromQuery] ListarClienteQuery query)
    {
        var result = await _listarClienteService.Handle(query);

        return Ok(result);
    }

    [HttpPost("", Name = "")]
    public async Task<ActionResult> CadastrarCliente([FromBody] CadastrarClienteCommand command)
    {
        await _cadastrarClienteService.Handle(command);

        return Ok();
    }
}