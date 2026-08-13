namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Clase base abstracta para todas las entidades del dominio que poseen una identidad única y eventos de dominio.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Lista interna encargada de almacenar los eventos de dominio generados por esta entidad.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Identificador único (UUID versión 7) de la entidad.
    /// </summary>
    public Guid Id { get; set; } = Guid.CreateVersion7();

    /// <summary>
    /// Colección de solo lectura de eventos de dominio pendientes de ser despachados.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Agrega un nuevo evento de dominio a la lista de eventos pendientes.
    /// </summary>
    /// <param name="domainEvent">Instancia del evento de dominio a registrar.</param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Remueve un evento de dominio específico de la lista de eventos pendientes.
    /// </summary>
    /// <param name="domainEvent">Instancia del evento de dominio a remover.</param>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Limpia todos los eventos de dominio almacenados en la entidad.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
