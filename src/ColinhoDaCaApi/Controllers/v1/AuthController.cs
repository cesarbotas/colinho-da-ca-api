using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Auth.v1.RefreshTokens;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
[ApiExplorerSettings(GroupName = "v1")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly ILoginService _loginService;
    private readonly IRegistrarService _registrarService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(ILogger<AuthController> logger,
        ILoginService loginService,
        IRegistrarService registrarService,
        IRefreshTokenService refreshTokenService)
    {
        _logger = logger;
        _loginService = loginService;
        _registrarService = registrarService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _loginService.Handle(command);

        return Ok(result);
    }

    [HttpPost("registrar")]
    public async Task<ActionResult> Registrar([FromBody] RegistrarCommand command)
    {
        await _registrarService.Handle(command);

        return Ok(new { mensagem = "Usuário registrado com sucesso" });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _refreshTokenService.Handle(command);

        return Ok(result);
    }
}