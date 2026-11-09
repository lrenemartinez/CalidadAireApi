using CalidadAireApi.Models;

namespace CalidadAireApi.Services;

public interface IAlertaService
{
    AlertaAire? Evaluar(LecturaAire lectura);
}