namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Interfaz marcadora para representar eventos que ocurren en el dominio del negocio.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Marca de tiempo en UTC indicando el momento exacto en el que ocurrió el evento de dominio.
    /// </summary>
    DateTimeOffset OccurredOn => DateTimeOffset.UtcNow;
}
