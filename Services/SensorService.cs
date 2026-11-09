using CalidadAireApi.Data;
using CalidadAireApi.Dto;
using CalidadAireApi.Exceptions;
using CalidadAireApi.Mapping;
using CalidadAireApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalidadAireApi.Services;

public class SensorService : ISensorService
{
    private readonly AppDbContext _contexto;

    public SensorService(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<SensorRespuestaDto>> ListarAsync(CancellationToken cancelacion)
    {
        var sensores = await _contexto.Sensores
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync(cancelacion);

        return sensores.Select(s => s.ToDto()).ToList();
    }

    public async Task<SensorRespuestaDto> ObtenerAsync(int id, CancellationToken cancelacion)
    {
        var sensor = await _contexto.Sensores.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancelacion)
            ?? throw new RecursoNoEncontradoException($"No existe el sensor con id {id}.");

        return sensor.ToDto();
    }

    public async Task<SensorRespuestaDto> CrearAsync(SensorEntradaDto dto, CancellationToken cancelacion)
    {
        var sensor = new SensorCalidadAire
        {
            Ubicacion = dto.Ubicacion.Trim(),
            TipoGas = dto.TipoGas.Trim(),
            Estado = dto.Estado
        };

        _contexto.Sensores.Add(sensor);
        await _contexto.SaveChangesAsync(cancelacion);

        return sensor.ToDto();
    }

    public async Task<SensorRespuestaDto> ActualizarAsync(int id, SensorEntradaDto dto, CancellationToken cancelacion)
    {
        var sensor = await _contexto.Sensores.FirstOrDefaultAsync(s => s.Id == id, cancelacion)
            ?? throw new RecursoNoEncontradoException($"No existe el sensor con id {id}.");

        sensor.Ubicacion = dto.Ubicacion.Trim();
        sensor.TipoGas = dto.TipoGas.Trim();
        sensor.Estado = dto.Estado;

        await _contexto.SaveChangesAsync(cancelacion);

        return sensor.ToDto();
    }
}