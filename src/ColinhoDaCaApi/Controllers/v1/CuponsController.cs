using ColinhoDaCa.Application.UseCases.Cupons.v1.AlterarCupom;
using ColinhoDaCa.Application.UseCases.Cupons.v1.CadastrarCupom;
using ColinhoDaCa.Application.UseCases.Cupons.v1.InativarCupom;
using ColinhoDaCa.Application.UseCases.Cupons.v1.ListarCupom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/cupons")]
[ApiExplorerSettings(GroupName = "v1")]
public class CuponsController : Controller
{
    private readonly ICadastrarCupomService _cadastrarCupomService;
    private readonly IListarCupomService _listarCupomService;
    private readonly IAlterarCupomService _alterarCupomService;
    private readonly IInativarCupomService _inativarCupomService;

    public CuponsController(
        ICadastrarCupomService cadastrarCupomService,
        IListarCupomService listarCupomService,
        IAlterarCupomService alterarCupomService,
        IInativarCupomService inativarCupomService)
    {
        _cadastrarCupomService = cadastrarCupomService;
        _listarCupomService = listarCupomService;
        _alterarCupomService = alterarCupomService;
        _inativarCupomService = inativarCupomService;
    }

    [HttpGet("")]
    public async Task<ActionResult> ListarCupons([FromQuery] ListarCupomQuery query)
    {
        var result = await _listarCupomService.Handle(query);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<ActionResult> CadastrarCupom([FromBody] CadastrarCupomCommand command)
    {
        await _cadastrarCupomService.Handle(command);
        return Ok(new { mensagem = "Cupom cadastrado com sucesso" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AlterarCupom([FromRoute] long id, [FromBody] AlterarCupomCommand command)
    {
        await _alterarCupomService.Handle(id, command);
        return Ok(new { mensagem = "Cupom alterado com sucesso" });
    }

    [HttpPost("{id}/inativar")]
    public async Task<ActionResult> InativarCupom([FromRoute] long id)
    {
        await _inativarCupomService.Handle(id);
        return Ok(new { mensagem = "Cupom inativado com sucesso" });
    }
}