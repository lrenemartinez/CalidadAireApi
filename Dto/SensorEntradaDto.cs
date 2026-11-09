using System.ComponentModel.DataAnnotations;
using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class SensorEntradaDto
{
    [Required, StringLength(150)]
    public string Ubicacion { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string TipoGas { get; set; } = string.Empty;

    public EstadoSensor Estado { get; set; } = EstadoSensor.Activo;
}