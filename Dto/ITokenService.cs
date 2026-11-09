using CalidadAireApi.Dto;
using CalidadAireApi.Models;

namespace CalidadAireApi.Services;

public interface ITokenService
{
    TokenRespuestaDto GenerarToken(Usuario usuario);
}