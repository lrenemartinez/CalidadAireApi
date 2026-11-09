using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class SensorRespuestaDto
{
    public int Id { get; set; }

    public string Ubicacion { get; set; } = string.Empty;

    public string TipoGas { get; set; } = string.Empty;

    public EstadoSensor Estado { get; set; }
}