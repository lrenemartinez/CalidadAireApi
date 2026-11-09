using System.ComponentModel.DataAnnotations;

namespace CalidadAireApi.Dto;

public class RegistroDto
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}