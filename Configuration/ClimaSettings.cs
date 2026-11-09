using System.ComponentModel.DataAnnotations;

namespace CalidadAireApi.Configuration;

public class ClimaSettings
{
    public const string Seccion = "Clima";

    [Required, Url]
    public string BaseUrl { get; set; } = string.Empty;

    [Range(-90, 90)]
    public double Latitud { get; set; }

    [Range(-180, 180)]
    public double Longitud { get; set; }

    [Range(1, 60)]
    public int TimeoutSegundos { get; set; } = 10;
}