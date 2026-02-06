using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
//[Authorize]
[Route("api/v1/clientes")]
[ApiExplorerSettings(GroupName = "v1")]
public class ClientesController : Controller
{
    [HttpPost("", Name = "")]
    public async Task<ActionResult> CadastrarCliente()
    {
        return Ok();
    }
}
