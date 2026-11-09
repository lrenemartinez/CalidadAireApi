using CalidadAireApi.Data;
using CalidadAireApi.Dto;
using CalidadAireApi.Enums;
using CalidadAireApi.Exceptions;
using CalidadAireApi.Mapping;
using CalidadAireApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalidadAireApi.Services;

public class LecturaService : ILecturaService
{
    private readonly AppDbContext _contexto;
    private readonly IAlertaService _alertaService;
    private readonly IClimaService _climaService;

    public LecturaService(AppDbContext contexto, IAlertaService alertaService, IClimaService climaService)
    {
        _contexto = contexto;
        _alertaService = alertaService;
        _climaService = climaService;
    }

    public async Task<LecturaRegistradaDto> RegistrarAsync(LecturaCrearDto dto, CancellationToken cancelacion)
    {
        var sensor = await _contexto.Sensores.AsNoTracking().FirstOrDefaultAsync(s => s.Id == dto.SensorId, cancelacion)
            ?? throw new RecursoNoEncontradoException($"No existe el sensor con id {dto.SensorId}.");

        if (sensor.Estado != EstadoSensor.Activo)
        {
            throw new ReglaNegocioException("El sensor no está activo y no puede registrar lecturas.");
        }

        var fecha = dto.FechaHora ?? DateTime.UtcNow;

        if (fecha > DateTime.UtcNow.AddMinutes(1))
        {
            throw new ReglaNegocioException("La fecha y hora de la lectura no puede estar en el futuro.");
        }

        var lectura = new LecturaAire
        {
            SensorId = sensor.Id,
            PM2_5 = dto.PM2_5,
            PM10 = dto.PM10,
            CO2 = dto.CO2,
            FechaHora = fecha
        };

        var alerta = _alertaService.Evaluar(lectura);

        _contexto.Lecturas.Add(lectura);

        if (alerta is not null)
        {
            _contexto.Alertas.Add(alerta);
        }

        await _contexto.SaveChangesAsync(cancelacion);

        return new LecturaRegistradaDto
        {
            Lectura = lectura.ToDto(),
            Alerta = alerta?.ToDto()
        };
    }

    public async Task<LecturaRespuestaDto> ObtenerAsync(int id, CancellationToken cancelacion)
    {
        var lectura = await _contexto.Lecturas.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancelacion)
            ?? throw new RecursoNoEncontradoException($"No existe la lectura con id {id}.");

        return lectura.ToDto();
    }

    public async Task<IReadOnlyList<LecturaContaminanteDto>> ConsultarAsync(LecturaFiltroDto filtro, CancellationToken cancelacion)
    {
        if (filtro.Desde is not { } desde || filtro.Hasta is not { } hasta || filtro.Contaminante is not { } tipo)
        {
            throw new ReglaNegocioException("Debe indicar las fechas 'desde' y 'hasta' y el tipo de contaminante.");
        }

        if (desde > hasta)
        {
            throw new ReglaNegocioException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
        }

        var consulta = _contexto.Lecturas
            .AsNoTracking()
            .Where(l => l.FechaHora >= desde && l.FechaHora <= hasta);

        if (filtro.SensorId is { } sensorId)
        {
            if (!await _contexto.Sensores.AnyAsync(s => s.Id == sensorId, cancelacion))
            {
                throw new RecursoNoEncontradoException($"No existe el sensor con id {sensorId}.");
            }

            consulta = consulta.Where(l => l.SensorId == sensorId);
        }

        var lecturas = await consulta
            .OrderBy(l => l.FechaHora)
            .ToListAsync(cancelacion);

        return lecturas.Select(l => l.ToDto(tipo)).ToList();
    }

    public async Task<LecturaEnriquecidaDto> ObtenerEnriquecidaAsync(int id, CancellationToken cancelacion)
    {
        var lectura = await _contexto.Lecturas
            .AsNoTracking()
            .Include(l => l.Sensor)
            .FirstOrDefaultAsync(l => l.Id == id, cancelacion)
            ?? throw new RecursoNoEncontradoException($"No existe la lectura con id {id}.");

        var clima = await _climaService.ObtenerClimaActualAsync(cancelacion);

        return new LecturaEnriquecidaDto
        {
            Lectura = lectura.ToDto(),
            Sensor = lectura.Sensor!.ToDto(),
            Clima = clima
        };
    }
}