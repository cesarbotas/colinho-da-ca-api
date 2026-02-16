using ColinhoDaCa.Application.UseCases.Racas.v1.Listar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColinhoDaCaApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class RacasController : ControllerBase
{
    private readonly ILogger<RacasController> _logger;
    private readonly IListarRacasService _listarRacasService;

    public RacasController(ILogger<RacasController> logger, 
        IListarRacasService listarRacasService)
    {
        _logger = logger;
        _listarRacasService = listarRacasService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] long? racaId)
    {
        var result = await _listarRacasService.Handle(racaId);
        return Ok(result);
    }
}