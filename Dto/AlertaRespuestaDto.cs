using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class AlertaRespuestaDto
{
    public int Id { get; set; }

    public int SensorId { get; set; }

    public int LecturaId { get; set; }

    public NivelAlerta Nivel { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; }
}