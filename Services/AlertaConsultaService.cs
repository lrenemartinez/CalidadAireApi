using CalidadAireApi.Data;
using CalidadAireApi.Dto;
using CalidadAireApi.Exceptions;
using CalidadAireApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CalidadAireApi.Services;

public class AlertaConsultaService : IAlertaConsultaService
{
    private readonly AppDbContext _contexto;

    public AlertaConsultaService(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<AlertaRespuestaDto>> ListarAsync(AlertaFiltroDto filtro, CancellationToken cancelacion)
    {
        if (filtro.Desde is { } inicio && filtro.Hasta is { } fin && inicio > fin)
        {
            throw new ReglaNegocioException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
        }

        var consulta = _contexto.Alertas.AsNoTracking().AsQueryable();

        if (filtro.SensorId is { } sensorId)
        {
            consulta = consulta.Where(a => a.SensorId == sensorId);
        }

        if (filtro.Nivel is { } nivel)
        {
            consulta = consulta.Where(a => a.Nivel == nivel);
        }

        if (filtro.Desde is { } desde)
        {
            consulta = consulta.Where(a => a.FechaHora >= desde);
        }

        if (filtro.Hasta is { } hasta)
        {
            consulta = consulta.Where(a => a.FechaHora <= hasta);
        }

        var alertas = await consulta
            .OrderByDescending(a => a.FechaHora)
            .ToListAsync(cancelacion);

        return alertas.Select(a => a.ToDto()).ToList();
    }
}