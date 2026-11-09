using CalidadAireApi.Dto;

namespace CalidadAireApi.Services;

public interface IClimaService
{
    Task<ClimaDto?> ObtenerClimaActualAsync(CancellationToken cancelacion);
}