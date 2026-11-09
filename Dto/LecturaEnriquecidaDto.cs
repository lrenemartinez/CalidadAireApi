namespace CalidadAireApi.Dto;

public class LecturaEnriquecidaDto
{
    public LecturaRespuestaDto Lectura { get; set; } = new();

    public SensorRespuestaDto Sensor { get; set; } = new();

    public ClimaDto? Clima { get; set; }
}