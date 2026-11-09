using System.ComponentModel.DataAnnotations;
using CalidadAireApi.Enums;

namespace CalidadAireApi.Models;

public class SensorCalidadAire
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Ubicacion { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string TipoGas { get; set; } = string.Empty;

    public EstadoSensor Estado { get; set; } = EstadoSensor.Activo;

    public ICollection<LecturaAire> Lecturas { get; set; } = new List<LecturaAire>();

    public ICollection<AlertaAire> Alertas { get; set; } = new List<AlertaAire>();
}