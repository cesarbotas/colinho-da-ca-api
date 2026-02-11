using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ExcluirReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/reservas")]
[ApiExplorerSettings(GroupName = "v1")]
public class ReservasController : Controller
{
    private readonly ILogger<ReservasController> _logger;
    private readonly ICadastrarReservaService _cadastrarReservaService;
    private readonly IListarReservaService _listarReservaService;
    private readonly IAlterarReservaService _alterarReservaService;
    private readonly IExcluirReservaService _excluirReservaService;

    public ReservasController(ILogger<ReservasController> logger,
        ICadastrarReservaService cadastrarReservaService,
        IListarReservaService listarReservaService,
        IAlterarReservaService alterarReservaService,
        IExcluirReservaService excluirReservaService)
    {
        _logger = logger;
        _cadastrarReservaService = cadastrarReservaService;
        _listarReservaService = listarReservaService;
        _alterarReservaService = alterarReservaService;
        _excluirReservaService = excluirReservaService;
    }

    [HttpGet("")]
    public async Task<ActionResult> ListarReservas([FromQuery] ListarReservaQuery query)
    {
        var result = await _listarReservaService.Handle(query);

        return Ok(result);
    }

    [HttpPost("")]
    public async Task<ActionResult> CadastrarReserva([FromBody] CadastrarReservaCommand command)
    {
        await _cadastrarReservaService.Handle(command);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AlterarReserva([FromRoute] long id, [FromBody] AlterarReservaCommand command)
    {
        await _alterarReservaService.Handle(id, command);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ExcluirReserva([FromRoute] long id)
    {
        await _excluirReservaService.Handle(id);

        return Ok();
    }
}
