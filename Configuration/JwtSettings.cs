using System.ComponentModel.DataAnnotations;

namespace CalidadAireApi.Configuration;

public class JwtSettings
{
    public const string Seccion = "Jwt";

    [Required, MinLength(32)]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenExpireMinutes { get; set; } = 30;
}