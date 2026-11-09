using CalidadAireApi.Dto;
using CalidadAireApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalidadAireApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SensoresController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensoresController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SensorRespuestaDto>>> Listar(CancellationToken cancelacion)
    {
        return Ok(await _sensorService.ListarAsync(cancelacion));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SensorRespuestaDto>> Obtener(int id, CancellationToken cancelacion)
    {
        return Ok(await _sensorService.ObtenerAsync(id, cancelacion));
    }

    [HttpPost]
    public async Task<ActionResult<SensorRespuestaDto>> Crear(SensorEntradaDto dto, CancellationToken cancelacion)
    {
        var sensor = await _sensorService.CrearAsync(dto, cancelacion);
        return CreatedAtAction(nameof(Obtener), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SensorRespuestaDto>> Actualizar(int id, SensorEntradaDto dto, CancellationToken cancelacion)
    {
        return Ok(await _sensorService.ActualizarAsync(id, dto, cancelacion));
    }
}