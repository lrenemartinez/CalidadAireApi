using System.Text.Json.Serialization;

namespace CalidadAireApi.Dto;

public class OpenMeteoRespuestaDto
{
    [JsonPropertyName("current")]
    public OpenMeteoActualDto? Actual { get; set; }
}

public class OpenMeteoActualDto
{
    [JsonPropertyName("temperature_2m")]
    public double? Temperatura { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public double? Humedad { get; set; }
}