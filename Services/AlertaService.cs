using CalidadAireApi.Enums;
using CalidadAireApi.Models;

namespace CalidadAireApi.Services;

public class AlertaService : IAlertaService
{
    private const decimal Pm25Leve = 25m;
    private const decimal Pm25Moderada = 50m;
    private const decimal Pm25Critica = 150m;
    private const decimal Pm25Extrema = 250m;
    private const decimal Pm10Critica = 200m;
    private const decimal Co2Moderada = 1000m;
    private const decimal Co2Extrema = 5000m;

    private const string MensajeLeve = "La calidad del aire es moderada, se recomienda reducir actividades al aire libre prolongadas.";
    private const string MensajeModerada = "La calidad del aire es poco saludable para grupos sensibles (niños, adultos mayores, personas con enfermedades respiratorias).";
    private const string MensajeCritica = "La calidad del aire es peligrosa. Se recomienda permanecer en interiores y usar mascarilla.";
    private const string MensajeExtrema = "Nivel de contaminación extremadamente alto. Riesgo severo para la salud.";

    public AlertaAire? Evaluar(LecturaAire lectura)
    {
        var clasificacion = Clasificar(lectura);

        if (clasificacion is null)
        {
            return null;
        }

        return new AlertaAire
        {
            SensorId = lectura.SensorId,
            Lectura = lectura,
            Nivel = clasificacion.Value.Nivel,
            Mensaje = clasificacion.Value.Mensaje,
            FechaHora = lectura.FechaHora
        };
    }

    private static (NivelAlerta Nivel, string Mensaje)? Clasificar(LecturaAire lectura)
    {
        if (lectura.CO2 > Co2Extrema || lectura.PM2_5 > Pm25Extrema)
        {
            return (NivelAlerta.Extrema, MensajeExtrema);
        }

        if (lectura.PM2_5 > Pm25Critica || lectura.PM10 > Pm10Critica)
        {
            return (NivelAlerta.Critica, MensajeCritica);
        }

        if (lectura.PM2_5 > Pm25Moderada || lectura.CO2 > Co2Moderada)
        {
            return (NivelAlerta.Moderada, MensajeModerada);
        }

        if (lectura.PM2_5 >= Pm25Leve)
        {
            return (NivelAlerta.Leve, MensajeLeve);
        }

        return null;
    }
}