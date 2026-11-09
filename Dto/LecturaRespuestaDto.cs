using System.Text.Json.Serialization;

namespace CalidadAireApi.Dto;

public class LecturaRespuestaDto
{
    public int Id { get; set; }

    public int SensorId { get; set; }

    [JsonPropertyName("pm2_5")]
    public decimal PM2_5 { get; set; }

    [JsonPropertyName("pm10")]
    public decimal PM10 { get; set; }

    [JsonPropertyName("co2")]
    public decimal CO2 { get; set; }

    public DateTime FechaHora { get; set; }
}
