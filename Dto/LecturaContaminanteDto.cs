using CalidadAireApi.Enums;

namespace CalidadAireApi.Dto;

public class LecturaContaminanteDto
{
    public int Id { get; set; }

    public int SensorId { get; set; }

    public DateTime FechaHora { get; set; }

    public TipoContaminante Contaminante { get; set; }

    public decimal Valor { get; set; }

    public string Unidad { get; set; } = string.Empty;
}