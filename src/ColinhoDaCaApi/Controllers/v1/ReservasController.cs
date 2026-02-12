using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.AprovarPagamento;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CancelarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ConcederDesconto;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ConfirmarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.EnviarComprovante;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ExcluirReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using ColinhoDaCa.Domain.Reservas.Repositories;
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
    private readonly IConfirmarReservaService _confirmarReservaService;
    private readonly IEnviarComprovanteService _enviarComprovanteService;
    private readonly IAprovarPagamentoService _aprovarPagamentoService;
    private readonly IConcederDescontoService _concederDescontoService;
    private readonly ICancelarReservaService _cancelarReservaService;

    public ReservasController(ILogger<ReservasController> logger,
        ICadastrarReservaService cadastrarReservaService,
        IListarReservaService listarReservaService,
        IAlterarReservaService alterarReservaService,
        IExcluirReservaService excluirReservaService,
        IConfirmarReservaService confirmarReservaService,
        IEnviarComprovanteService enviarComprovanteService,
        IAprovarPagamentoService aprovarPagamentoService,
        IConcederDescontoService concederDescontoService,
        ICancelarReservaService cancelarReservaService)
    {
        _logger = logger;
        _cadastrarReservaService = cadastrarReservaService;
        _listarReservaService = listarReservaService;
        _alterarReservaService = alterarReservaService;
        _excluirReservaService = excluirReservaService;
        _confirmarReservaService = confirmarReservaService;
        _enviarComprovanteService = enviarComprovanteService;
        _aprovarPagamentoService = aprovarPagamentoService;
        _concederDescontoService = concederDescontoService;
        _cancelarReservaService = cancelarReservaService;
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

    [HttpPost("{id}/confirmar")]
    public async Task<ActionResult> ConfirmarReserva([FromRoute] long id)
    {
        await _confirmarReservaService.Handle(id);

        return Ok(new { mensagem = "Reserva confirmada e email enviado ao cliente" });
    }

    [HttpPost("{id}/comprovante")]
    [Consumes("application/json")]
    public async Task<ActionResult> EnviarComprovante([FromRoute] long id, [FromBody] EnviarComprovanteCommand command)
    {
        await _enviarComprovanteService.Handle(id, command);

        return Ok(new { mensagem = "Comprovante enviado com sucesso" });
    }

    [HttpPost("{id}/aprovar-pagamento")]
    public async Task<ActionResult> AprovarPagamento([FromRoute] long id)
    {
        await _aprovarPagamentoService.Handle(id);

        return Ok(new { mensagem = "Pagamento aprovado e email de confirmação enviado" });
    }

    [HttpGet("{id}/comprovante")]
    public async Task<ActionResult> VisualizarComprovante([FromRoute] long id, [FromServices] IReservaRepository reservaRepository)
    {
        var reserva = await reservaRepository.GetAsync(r => r.Id == id);

        if (reserva == null)
        {
            return NotFound(new { mensagem = "Reserva não encontrada" });
        }

        if (string.IsNullOrEmpty(reserva.ComprovantePagamento))
        {
            return NotFound(new { mensagem = "Comprovante não encontrado" });
        }

        return Ok(new
        {
            comprovantePagamento = reserva.ComprovantePagamento,
            observacoesPagamento = reserva.ObservacoesPagamento,
            dataPagamento = reserva.DataPagamento
        });
    }

    [HttpPost("{id}/desconto")]
    [Consumes("application/json")]
    public async Task<ActionResult> ConcederDesconto([FromRoute] long id, [FromBody] ConcederDescontoCommand command)
    {
        await _concederDescontoService.Handle(id, command);

        return Ok(new { mensagem = "Desconto concedido com sucesso" });
    }

    [HttpPost("{id}/cancelar")]
    public async Task<ActionResult> CancelarReserva([FromRoute] long id)
    {
        await _cancelarReservaService.Handle(id);

        return Ok(new { mensagem = "Reserva cancelada com sucesso" });
    }
}