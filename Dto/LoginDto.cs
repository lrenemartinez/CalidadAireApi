using System.ComponentModel.DataAnnotations;

namespace CalidadAireApi.Dto;

public class LoginDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
