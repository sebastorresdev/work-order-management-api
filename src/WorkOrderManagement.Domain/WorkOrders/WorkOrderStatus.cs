namespace WorkOrderManagement.Domain.WorkOrders;

/// <summary>
/// Estados de la solicitud u orden de trabajo.
/// </summary>
public enum WorkOrderStatus
{
    /// <summary>
    /// Registrado por el Vendedor, en espera de revisión por el Backoffice.
    /// </summary>
    Pendiente = 1,

    /// <summary>
    /// Observado por el Backoffice debido a datos faltantes o correcciones requeridas.
    /// </summary>
    Observado = 2,

    /// <summary>
    /// Programado por el Backoffice con fecha, turno y técnico asignado.
    /// </summary>
    Agendado = 3,

    /// <summary>
    /// Trabajo atendido y finalizado exitosamente por el Backoffice.
    /// </summary>
    Completado = 4,

    /// <summary>
    /// Desestimado o rechazado por el Backoffice (ej. sin cobertura, cliente no aplica, duplicado).
    /// </summary>
    Rechazado = 5,

    /// <summary>
    /// Cancelado por el Vendedor o por solicitud del cliente.
    /// </summary>
    Cancelado = 6
}
