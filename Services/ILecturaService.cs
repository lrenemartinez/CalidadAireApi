using CalidadAireApi.Dto;

namespace CalidadAireApi.Services;

public interface ILecturaService
{
    Task<LecturaRegistradaDto> RegistrarAsync(LecturaCrearDto dto, CancellationToken cancelacion);

    Task<LecturaRespuestaDto> ObtenerAsync(int id, CancellationToken cancelacion);

    Task<IReadOnlyList<LecturaContaminanteDto>> ConsultarAsync(LecturaFiltroDto filtro, CancellationToken cancelacion);

    Task<LecturaEnriquecidaDto> ObtenerEnriquecidaAsync(int id, CancellationToken cancelacion);
}