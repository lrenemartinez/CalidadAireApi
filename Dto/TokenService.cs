using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalidadAireApi.Configuration;
using CalidadAireApi.Dto;
using CalidadAireApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CalidadAireApi.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwt;

    public TokenService(IOptions<JwtSettings> opciones)
    {
        _jwt = opciones.Value;
    }

    public TokenRespuestaDto GenerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpireMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales);

        return new TokenRespuestaDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEn = expiracion
        };
    }
}