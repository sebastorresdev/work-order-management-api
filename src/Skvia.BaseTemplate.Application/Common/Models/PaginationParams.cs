namespace Skvia.BaseTemplate.Application.Common.Models;

/// <summary>
/// Parámetros base para solicitudes de listados paginados y búsquedas.
/// </summary>
public record PaginationParams
{
    /// <summary>
    /// Tamaño máximo permitido de elementos devueltos en una sola página.
    /// </summary>
    private const int MaxPageSize = 100;

    /// <summary>
    /// Campo privado para almacenar el tamaño de página configurado.
    /// </summary>
    private int _pageSize = 10;

    /// <summary>
    /// Número de página solicitada (por defecto 1).
    /// </summary>
    public int PageNumber { init; get; } = 1;

    /// <summary>
    /// Cantidad de elementos solicitados por página, con límites máximos y mínimos sanitizados.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value <= 0 ? 10 : value;
    }

    /// <summary>
    /// Término o filtro de búsqueda textual para filtrar los resultados (opcional).
    /// </summary>
    public string? SearchTerm { init; get; }
}

