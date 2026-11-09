using CalidadAireApi.Dto;
using CalidadAireApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalidadAireApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AlertasController : ControllerBase
{
    private readonly IAlertaConsultaService _alertaConsultaService;

    public AlertasController(IAlertaConsultaService alertaConsultaService)
    {
        _alertaConsultaService = alertaConsultaService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AlertaRespuestaDto>>> Listar([FromQuery] AlertaFiltroDto filtro, CancellationToken cancelacion)
    {
        return Ok(await _alertaConsultaService.ListarAsync(filtro, cancelacion));
    }
}