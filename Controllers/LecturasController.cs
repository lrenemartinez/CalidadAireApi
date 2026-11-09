using CalidadAireApi.Dto;
using CalidadAireApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalidadAireApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LecturasController : ControllerBase
{
    private readonly ILecturaService _lecturaService;

    public LecturasController(ILecturaService lecturaService)
    {
        _lecturaService = lecturaService;
    }

    [HttpPost]
    public async Task<ActionResult<LecturaRegistradaDto>> Registrar(LecturaCrearDto dto, CancellationToken cancelacion)
    {
        var resultado = await _lecturaService.RegistrarAsync(dto, cancelacion);
        return CreatedAtAction(nameof(Obtener), new { id = resultado.Lectura.Id }, resultado);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LecturaContaminanteDto>>> Consultar([FromQuery] LecturaFiltroDto filtro, CancellationToken cancelacion)
    {
        return Ok(await _lecturaService.ConsultarAsync(filtro, cancelacion));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LecturaRespuestaDto>> Obtener(int id, CancellationToken cancelacion)
    {
        return Ok(await _lecturaService.ObtenerAsync(id, cancelacion));
    }
    [HttpGet("{id:int}/enriquecida")]
    public async Task<ActionResult<LecturaEnriquecidaDto>> ObtenerEnriquecida(int id, CancellationToken cancelacion)
    {
        return Ok(await _lecturaService.ObtenerEnriquecidaAsync(id, cancelacion));
    }
}