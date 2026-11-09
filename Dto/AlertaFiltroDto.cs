using System.ComponentModel.DataAnnotations;
using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class AlertaFiltroDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El sensorId debe ser mayor que cero.")]
    public int? SensorId { get; set; }

    public NivelAlerta? Nivel { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }
}