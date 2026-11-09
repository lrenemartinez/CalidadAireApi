using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using CalidadAireApi.Configuration;
using CalidadAireApi.Dto;
using Microsoft.Extensions.Options;

namespace CalidadAireApi.Services;

public class ClimaService : IClimaService
{
    private const string Fuente = "Open-Meteo";

    private readonly HttpClient _httpClient;
    private readonly ClimaSettings _configuracion;
    private readonly ILogger<ClimaService> _logger;

    public ClimaService(HttpClient httpClient, IOptions<ClimaSettings> opciones, ILogger<ClimaService> logger)
    {
        _httpClient = httpClient;
        _configuracion = opciones.Value;
        _logger = logger;
    }

    public async Task<ClimaDto?> ObtenerClimaActualAsync(CancellationToken cancelacion)
    {
        var ruta = string.Create(
            CultureInfo.InvariantCulture,
            $"v1/forecast?latitude={_configuracion.Latitud}&longitude={_configuracion.Longitud}&current=temperature_2m,relative_humidity_2m");

        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<OpenMeteoRespuestaDto>(ruta, cancelacion);

            if (respuesta?.Actual is not { Temperatura: { } temperatura, Humedad: { } humedad })
            {
                _logger.LogWarning("La respuesta de {Fuente} no incluyó temperatura y humedad.", Fuente);
                return null;
            }

            return new ClimaDto
            {
                TemperaturaCelsius = temperatura,
                HumedadRelativaPorcentaje = humedad,
                Fuente = Fuente,
                ConsultadoEn = DateTime.UtcNow
            };
        }
        catch (Exception ex) when (!cancelacion.IsCancellationRequested
            && (ex is HttpRequestException or TaskCanceledException or JsonException))
        {
            _logger.LogWarning(ex, "No fue posible consultar el clima en {Fuente}.", Fuente);
            return null;
        }
    }
}