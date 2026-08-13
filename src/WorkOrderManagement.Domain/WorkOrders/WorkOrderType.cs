namespace WorkOrderManagement.Domain.WorkOrders;

/// <summary>
/// Tipo de solicitud u orden de trabajo.
/// </summary>
public enum WorkOrderType
{
    /// <summary>
    /// Instalación de servicios, decodificadores o infraestructura.
    /// </summary>
    Instalacion = 1,

    /// <summary>
    /// Reporte de avería, fallo técnico o reparación.
    /// </summary>
    Averia = 2,

    /// <summary>
    /// Envío o entrega de encomienda o suministros.
    /// </summary>
    Encomienda = 3
}
