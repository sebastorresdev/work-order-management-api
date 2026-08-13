using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Infrastructure.Services;

/// <summary>
/// Implementación predeterminada del reloj del sistema que provee la hora UTC actual.
/// </summary>
public class SystemClock : IClock
{
    /// <summary>
    /// Retorna la fecha y hora actual del servidor en tiempo universal coordinado (UTC).
    /// </summary>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

