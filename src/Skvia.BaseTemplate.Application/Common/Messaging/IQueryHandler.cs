namespace Skvia.BaseTemplate.Application.Common.Messaging;

/// <summary>
/// Contrato para los manejadores encubiertos del procesamiento de consultas (Queries).
/// </summary>
/// <typeparam name="TQuery">Tipo de consulta a procesar.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta retornado.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : IErrorOr
{
    /// <summary>
    /// Ejcuta de forma asíncrona la consulta especificada.
    /// </summary>
    /// <param name="query">Instancia de la consulta con parámetros de filtro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Tarea asíncrona con el resultado obtenido.</returns>
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

