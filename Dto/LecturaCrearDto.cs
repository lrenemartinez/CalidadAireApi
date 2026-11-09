using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalidadAireApi.Dto;

public class LecturaCrearDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El sensorId debe ser mayor que cero.")]
    public int SensorId { get; set; }

    [JsonPropertyName("pm2_5")]
    [Range(0, 100000, ErrorMessage = "PM2.5 no puede ser negativo.")]
    public decimal PM2_5 { get; set; }

    [JsonPropertyName("pm10")]
    [Range(0, 100000, ErrorMessage = "PM10 no puede ser negativo.")]
    public decimal PM10 { get; set; }

    [JsonPropertyName("co2")]
    [Range(0, 100000, ErrorMessage = "CO2 no puede ser negativo.")]
    public decimal CO2 { get; set; }

    public DateTime? FechaHora { get; set; }
}