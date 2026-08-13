using Microsoft.EntityFrameworkCore;

namespace Skvia.BaseTemplate.Application.Common.Models;

/// <summary>
/// Métodos de extensión para consultas de Entity Framework Core (<see cref="IQueryable{T}"/>) facilitando la paginación.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Convierte de forma asíncrona una consulta <see cref="IQueryable{T}"/> en una respuesta paginada <see cref="PaginatedResponse{T}"/>.
    /// </summary>
    /// <typeparam name="T">Tipo del modelo de la consulta.</typeparam>
    /// <param name="query">Consulta IQueryable de entrada.</param>
    /// <param name="pageNumber">Número de página solicitada.</param>
    /// <param name="pageSize">Tamaño o cantidad de elementos por página.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Instancia de <see cref="PaginatedResponse{T}"/> con los elementos de la página y metadatos de conteo.</returns>
    public static async Task<PaginatedResponse<T>> ToPaginatedResponseAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // Conteo total de elementos sin aplicar Skip ni Take
        var totalCount = await query.CountAsync(cancellationToken);
        // Garantiza que la página mínima sea 1
        var currentPage = pageNumber < 1 ? 1 : pageNumber;
        // Garantiza que el tamaño de página mínimo sea 10
        var validPageSize = pageSize <= 0 ? 10 : pageSize;

        // Obtención de la sublista correspondiente a la página solicitada
        var items = await query
            .Skip((currentPage - 1) * validPageSize)
            .Take(validPageSize)
            .ToListAsync(cancellationToken);

        return PaginatedResponse<T>.Create(items, totalCount, currentPage, validPageSize);
    }
}

