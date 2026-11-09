using CalidadAireApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CalidadAireApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (estado, titulo) = exception switch
        {
            RecursoNoEncontradoException => (StatusCodes.Status404NotFound, "Recurso no encontrado."),
            ReglaNegocioException => (StatusCodes.Status400BadRequest, "Solicitud inválida."),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor.")
        };

        if (estado == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Error no controlado en {Ruta}", httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = estado;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = estado,
            Title = titulo,
            Detail = estado == StatusCodes.Status500InternalServerError ? null : exception.Message
        }, cancellationToken);

        return true;
    }
}