using System.ComponentModel.DataAnnotations;
using CalidadAireApi.Enums;

namespace CalidadAireApi.Models;

public class AlertaAire
{
    public int Id { get; set; }

    public int SensorId { get; set; }

    public SensorCalidadAire? Sensor { get; set; }

    public int LecturaId { get; set; }

    public LecturaAire? Lectura { get; set; }

    public NivelAlerta Nivel { get; set; }

    [Required, StringLength(300)]
    public string Mensaje { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; }
}