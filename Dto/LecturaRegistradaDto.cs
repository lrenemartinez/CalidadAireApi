namespace CalidadAireApi.Dto;

public class LecturaRegistradaDto
{
    public LecturaRespuestaDto Lectura { get; set; } = new();

    public AlertaRespuestaDto? Alerta { get; set; }
}