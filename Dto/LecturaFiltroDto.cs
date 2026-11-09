using System.ComponentModel.DataAnnotations;
using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class LecturaFiltroDto
{
    [Required(ErrorMessage = "La fecha 'desde' es obligatoria.")]
    public DateTime? Desde { get; set; }

    [Required(ErrorMessage = "La fecha 'hasta' es obligatoria.")]
    public DateTime? Hasta { get; set; }

    [Required(ErrorMessage = "El tipo de contaminante es obligatorio.")]
    public TipoContaminante? Contaminante { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El sensorId debe ser mayor que cero.")]
    public int? SensorId { get; set; }
}