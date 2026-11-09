using CalidadAireApi.Dto;

namespace CalidadAireApi.Services;

public interface ISensorService
{
    Task<IReadOnlyList<SensorRespuestaDto>> ListarAsync(CancellationToken cancelacion);

    Task<SensorRespuestaDto> ObtenerAsync(int id, CancellationToken cancelacion);

    Task<SensorRespuestaDto> CrearAsync(SensorEntradaDto dto, CancellationToken cancelacion);

    Task<SensorRespuestaDto> ActualizarAsync(int id, SensorEntradaDto dto, CancellationToken cancelacion);
}
