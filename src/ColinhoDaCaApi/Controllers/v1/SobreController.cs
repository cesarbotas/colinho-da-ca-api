using ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Route("api/v1/sobre")]
[ApiExplorerSettings(GroupName = "v1")]
public class SobreController : Controller
{
    private readonly ILogger<SobreController> _logger;
    private readonly IEnviarEmailService _enviarEmailService;

    public SobreController(ILogger<SobreController> logger, 
        IEnviarEmailService enviarEmailService)
    {
        _logger = logger;
        _enviarEmailService = enviarEmailService;
    }

    [HttpPost("enviar-email")]
    public async Task<ActionResult> EnviarEmail([FromBody] EnviarEmailCommand command)
    {
        await _enviarEmailService.Handle(command);

        return Ok(new { mensagem = "Email enviado com sucesso" });
    }
}