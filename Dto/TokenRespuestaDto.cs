namespace CalidadAireApi.Dto;

public class TokenRespuestaDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEn { get; set; }
}