namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Abstracción del reloj del sistema para la obtención desacoplada de la fecha y hora UTC.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Obtiene la fecha y hora actual en UTC.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}

