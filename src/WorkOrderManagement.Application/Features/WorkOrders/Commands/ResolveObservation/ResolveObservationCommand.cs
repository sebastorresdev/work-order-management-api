using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation;

/// <summary>
/// Comando CQRS para responder y levantar la observación de una solicitud de servicio.
/// Permite al vendedor ingresar la nota de subsanación y regresar la orden al estado Pendiente.
/// </summary>
/// <param name="WorkOrderId">Identificador único de la orden de trabajo observada.</param>
/// <param name="ResolutionNotes">Nota o explicación ingresada por el vendedor detallando la corrección.</param>
/// <param name="UpdatedByUserId">Identificador del usuario vendedor que ejecuta la subsanación.</param>
public record ResolveObservationCommand(
    Guid WorkOrderId,
    string ResolutionNotes,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
