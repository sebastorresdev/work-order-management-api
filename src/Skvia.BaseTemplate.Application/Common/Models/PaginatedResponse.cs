namespace Skvia.BaseTemplate.Application.Common.Models;

/// <summary>
/// Modelo de respuesta genérico estructurado para soportar listas paginadas de elementos.
/// </summary>
/// <typeparam name="T">Tipo de los elementos contenidos en la página.</typeparam>
/// <param name="Items">Lista de elementos pertenecientes a la página actual.</param>
/// <param name="PageNumber">Número de página actual (base 1).</param>
/// <param name="PageSize">Cantidad máxima de elementos solicitados por página.</param>
/// <param name="TotalCount">Total absoluto de elementos disponibles en la consulta sin paginar.</param>
/// <param name="TotalPages">Cantidad total calculada de páginas disponibles.</param>
/// <param name="HasPreviousPage">Indica si existe una página previa a la actual.</param>
/// <param name="HasNextPage">Indica si existe una página posterior a la actual.</param>
public record PaginatedResponse<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage)
{
    /// <summary>
    /// Crea y calcula una respuesta paginada a partir de los datos y totales provistos.
    /// </summary>
    /// <param name="items">Elementos contenidos en la página actual.</param>
    /// <param name="totalCount">Conteo total de elementos.</param>
    /// <param name="pageNumber">Número de página actual.</param>
    /// <param name="pageSize">Tamaño de la página.</param>
    /// <returns>Una instancia de <see cref="PaginatedResponse{T}"/> perfectamente calculada.</returns>
    public static PaginatedResponse<T> Create(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        // Cálculo del total de páginas dividiendo el total entre el tamaño de página
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        // Garantiza que el número de página sea al menos 1
        var currentPage = pageNumber < 1 ? 1 : pageNumber;

        return new PaginatedResponse<T>(
            items,
            currentPage,
            pageSize,
            totalCount,
            totalPages,
            HasPreviousPage: currentPage > 1,
            HasNextPage: currentPage < totalPages);
    }
}

