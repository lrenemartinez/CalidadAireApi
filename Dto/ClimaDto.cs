namespace CalidadAireApi.Dto;

public class ClimaDto
{
    public double TemperaturaCelsius { get; set; }

    public double HumedadRelativaPorcentaje { get; set; }

    public string Fuente { get; set; } = string.Empty;

    public DateTime ConsultadoEn { get; set; }
}
