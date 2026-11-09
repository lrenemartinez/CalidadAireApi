using CalidadAireApi.Dto;
using CalidadAireApi.Enums;
using CalidadAireApi.Models;

namespace CalidadAireApi.Mapping;

public static class MapeoExtensions
{
    public static SensorRespuestaDto ToDto(this SensorCalidadAire sensor)
    {
        return new SensorRespuestaDto
        {
            Id = sensor.Id,
            Ubicacion = sensor.Ubicacion,
            TipoGas = sensor.TipoGas,
            Estado = sensor.Estado
        };
    }

    public static LecturaRespuestaDto ToDto(this LecturaAire lectura)
    {
        return new LecturaRespuestaDto
        {
            Id = lectura.Id,
            SensorId = lectura.SensorId,
            PM2_5 = lectura.PM2_5,
            PM10 = lectura.PM10,
            CO2 = lectura.CO2,
            FechaHora = lectura.FechaHora
        };
    }

    public static AlertaRespuestaDto ToDto(this AlertaAire alerta)
    {
        return new AlertaRespuestaDto
        {
            Id = alerta.Id,
            SensorId = alerta.SensorId,
            LecturaId = alerta.LecturaId,
            Nivel = alerta.Nivel,
            Mensaje = alerta.Mensaje,
            FechaHora = alerta.FechaHora
        };
    }

    public static LecturaContaminanteDto ToDto(this LecturaAire lectura, TipoContaminante tipo)
    {
        return new LecturaContaminanteDto
        {
            Id = lectura.Id,
            SensorId = lectura.SensorId,
            FechaHora = lectura.FechaHora,
            Contaminante = tipo,
            Valor = tipo switch
            {
                TipoContaminante.Pm25 => lectura.PM2_5,
                TipoContaminante.Pm10 => lectura.PM10,
                TipoContaminante.Co2 => lectura.CO2,
                _ => throw new ArgumentOutOfRangeException(nameof(tipo))
            },
            Unidad = tipo == TipoContaminante.Co2 ? "ppm" : "µg/m³"
        };
    }
}