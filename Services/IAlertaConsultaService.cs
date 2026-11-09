using CalidadAireApi.Dto;

namespace CalidadAireApi.Services;

public interface IAlertaConsultaService
{
    Task<IReadOnlyList<AlertaRespuestaDto>> ListarAsync(AlertaFiltroDto filtro, CancellationToken cancelacion);
}