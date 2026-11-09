using CalidadAireApi.Data;
using CalidadAireApi.Dto;
using CalidadAireApi.Models;
using CalidadAireApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalidadAireApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _contexto;
    private readonly IPasswordHasher<Usuario> _hasher;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext contexto, IPasswordHasher<Usuario> hasher, ITokenService tokenService)
    {
        _contexto = contexto;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto, CancellationToken cancelacion)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        if (await _contexto.Usuarios.AnyAsync(u => u.Correo == correo, cancelacion))
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "El correo ya está registrado.");
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Correo = correo
        };
        usuario.PasswordHash = _hasher.HashPassword(usuario, dto.Password);

        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync(cancelacion);

        return StatusCode(StatusCodes.Status201Created, new { usuario.Id, usuario.Nombre, usuario.Correo });
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenRespuestaDto>> Login(LoginDto dto, CancellationToken cancelacion)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo, cancelacion);

        if (usuario is null ||
            _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Credenciales inválidas.");
        }

        return Ok(_tokenService.GenerarToken(usuario));
    }
}