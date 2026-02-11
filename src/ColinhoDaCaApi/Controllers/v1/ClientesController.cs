using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/clientes")]
[ApiExplorerSettings(GroupName = "v1")]
public class ClientesController : Controller
{
    private readonly ILogger<ClientesController> _logger;
    private readonly ICadastrarClienteService _cadastrarClienteService;
    private readonly IListarClienteService _listarClienteService;
    private readonly IAlterarClienteService _alterarClienteService;
    private readonly IExcluirClienteService _excluirClienteService;

    public ClientesController(ILogger<ClientesController> logger,
        ICadastrarClienteService cadastrarClienteService,
        IListarClienteService listarClienteService,
        IAlterarClienteService alterarClienteService,
        IExcluirClienteService excluirClienteService)
    {
        _logger = logger;
        _cadastrarClienteService = cadastrarClienteService;
        _listarClienteService = listarClienteService;
        _alterarClienteService = alterarClienteService;
        _excluirClienteService = excluirClienteService;
    }

    [HttpGet("", Name = "")]
    public async Task<ActionResult<ResultadoPaginadoDto<ClientesDto>>> ListarClientes([FromQuery] ListarClienteQuery query)
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

    [HttpPut("{id}", Name = "AlterarCliente")]
    public async Task<ActionResult> AlterarCliente([FromRoute] long id, [FromBody] AlterarClienteCommand command)
    {
        await _alterarClienteService.Handle(id, command);

        return Ok();
    }

    [HttpDelete("{id}", Name = "ExcluirCliente")]
    public async Task<ActionResult> ExcluirCliente([FromRoute] long id)
    {
        await _excluirClienteService.Handle(id);

        return Ok();
    }
}