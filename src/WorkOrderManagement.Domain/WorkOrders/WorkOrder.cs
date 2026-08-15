using ErrorOr;

using WorkOrderManagement.Domain.Branches;
using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;

namespace WorkOrderManagement.Domain.WorkOrders;

/// <summary>
/// Entidad de dominio principal que representa una Solicitud de Servicio / Orden de Trabajo.
/// </summary>
public class WorkOrder : BaseAuditableEntity
{
    private readonly List<WorkOrderStatusHistory> _statusHistory = [];
    private readonly List<WorkOrderScheduleHistory> _scheduleHistory = [];

    /// <summary>
    /// Código de ticket correlativo único (ej: SOL-2026-00001).
    /// </summary>
    public string TicketNumber { get; private set; } = null!;

    /// <summary>
    /// Tipo de solicitud (Instalación, Avería, Encomienda).
    /// </summary>
    public WorkOrderType RequestType { get; private set; }

    /// <summary>
    /// Estado actual de la orden.
    /// </summary>
    public WorkOrderStatus Status { get; private set; }

    /// <summary>
    /// Prioridad de la atención.
    /// </summary>
    public WorkOrderPriority Priority { get; private set; }

    /// <summary>
    /// Sede a la que pertenece la solicitud.
    /// </summary>
    public Guid BranchId { get; private set; }
    public virtual Branch Branch { get; private set; } = null!;

    /// <summary>
    /// Usuario Vendedor que creó la solicitud.
    /// </summary>
    public Guid CreatedByUserId { get; private set; }
    public virtual ApplicationUser CreatedByUser { get; private set; } = null!;

    // Información del Cliente
    public string ClientCode { get; private set; } = null!;
    public string ClientName { get; private set; } = null!;
    public string ClientPhone { get; private set; } = null!;
    public string? ClientSecondaryPhone { get; private set; }
    public string District { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string? AddressReference { get; private set; }
    public string Description { get; private set; } = null!;

    // Datos de Agendamiento Actual
    public DateOnly? ScheduledDate { get; private set; }
    public string? ScheduledSlot { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }
    public virtual ApplicationUser? AssignedTechnician { get; private set; }

    // Datos de Cierre u Observaciones
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? CompletionNotes { get; private set; }
    public string? ObservationNotes { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? CancellationReason { get; private set; }

    // Colecciones de Navegación
    public virtual IReadOnlyCollection<WorkOrderStatusHistory> StatusHistory => _statusHistory.AsReadOnly();
    public virtual IReadOnlyCollection<WorkOrderScheduleHistory> ScheduleHistory => _scheduleHistory.AsReadOnly();

    private WorkOrder() { }

    public static ErrorOr<WorkOrder> Create(
        string ticketNumber,
        WorkOrderType requestType,
        WorkOrderPriority priority,
        Guid branchId,
        Guid createdByUserId,
        string clientCode,
        string clientName,
        string clientPhone,
        string district,
        string address,
        string description,
        string? clientSecondaryPhone = null,
        string? addressReference = null)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(ticketNumber))
            errors.Add(Error.Validation("WorkOrder.TicketNumberRequired", "El número de ticket es requerido."));

        if (string.IsNullOrWhiteSpace(clientCode))
            errors.Add(Error.Validation("WorkOrder.ClientCodeRequired", "El código de cliente es requerido."));

        if (string.IsNullOrWhiteSpace(clientName))
            errors.Add(Error.Validation("WorkOrder.ClientNameRequired", "El nombre de cliente es requerido."));

        if (string.IsNullOrWhiteSpace(clientPhone))
            errors.Add(Error.Validation("WorkOrder.ClientPhoneRequired", "El teléfono de contacto es requerido."));

        if (string.IsNullOrWhiteSpace(district))
            errors.Add(Error.Validation("WorkOrder.DistrictRequired", "El distrito es requerido."));

        if (string.IsNullOrWhiteSpace(address))
            errors.Add(Error.Validation("WorkOrder.AddressRequired", "La dirección es requerida."));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(Error.Validation("WorkOrder.DescriptionRequired", "La descripción de la solicitud es requerida."));

        if (errors.Count > 0) return errors;

        var workOrder = new WorkOrder
        {
            Id = Guid.NewGuid(),
            TicketNumber = ticketNumber.Trim().ToUpper(),
            RequestType = requestType,
            Status = WorkOrderStatus.Pendiente,
            Priority = priority,
            BranchId = branchId,
            CreatedByUserId = createdByUserId,
            ClientCode = clientCode.Trim(),
            ClientName = clientName.Trim(),
            ClientPhone = clientPhone.Trim(),
            ClientSecondaryPhone = clientSecondaryPhone?.Trim(),
            District = district.Trim(),
            Address = address.Trim(),
            AddressReference = addressReference?.Trim(),
            Description = description.Trim(),
            Created = DateTimeOffset.UtcNow,
            CreatedBy = createdByUserId
        };

        workOrder._statusHistory.Add(WorkOrderStatusHistory.Create(
            workOrder.Id,
            WorkOrderStatus.Pendiente,
            WorkOrderStatus.Pendiente,
            "Solicitud registrada por el Vendedor.",
            createdByUserId));

        return workOrder;
    }

    public ErrorOr<Success> UpdateInfo(
        WorkOrderType requestType,
        WorkOrderPriority priority,
        string clientCode,
        string clientName,
        string clientPhone,
        string district,
        string address,
        string description,
        string? clientSecondaryPhone,
        string? addressReference,
        Guid updatedByUserId)
    {
        if (Status != WorkOrderStatus.Pendiente && Status != WorkOrderStatus.Observado)
        {
            return Error.Validation("WorkOrder.InvalidStateForEdit", "Solo se pueden editar solicitudes en estado Pendiente u Observado.");
        }

        ClientCode = clientCode.Trim();
        ClientName = clientName.Trim();
        ClientPhone = clientPhone.Trim();
        ClientSecondaryPhone = clientSecondaryPhone?.Trim();
        District = district.Trim();
        Address = address.Trim();
        AddressReference = addressReference?.Trim();
        Description = description.Trim();
        RequestType = requestType;
        Priority = priority;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        // Si estaba observada y el vendedor la edita, pasa nuevamente a Pendiente
        if (Status == WorkOrderStatus.Observado)
        {
            var oldStatus = Status;
            Status = WorkOrderStatus.Pendiente;
            ObservationNotes = null;

            _statusHistory.Add(WorkOrderStatusHistory.Create(
                Id,
                oldStatus,
                WorkOrderStatus.Pendiente,
                "Información corregida por el vendedor. Pasa a revisión de Backoffice.",
                updatedByUserId));
        }

        return Result.Success;
    }

    public ErrorOr<Success> Schedule(
        DateOnly scheduledDate,
        string scheduledSlot,
        Guid? assignedTechnicianId,
        string? notes,
        Guid updatedByUserId)
    {
        if (Status == WorkOrderStatus.Completado || Status == WorkOrderStatus.Cancelado || Status == WorkOrderStatus.Rechazado)
        {
            return Error.Validation("WorkOrder.InvalidStateForSchedule", "No se puede agendar una orden completada, rechazada o cancelada.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Agendado;
        ScheduledDate = scheduledDate;
        ScheduledSlot = scheduledSlot.Trim();
        AssignedTechnicianId = assignedTechnicianId;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _scheduleHistory.Add(WorkOrderScheduleHistory.Create(
            Id,
            scheduledDate,
            scheduledSlot,
            assignedTechnicianId,
            notes,
            updatedByUserId));

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Agendado,
            $"Programado para el {scheduledDate:dd/MM/yyyy} ({scheduledSlot}). {notes}".Trim(),
            updatedByUserId));

        return Result.Success;
    }

    public ErrorOr<Success> Observe(string reason, Guid updatedByUserId)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Error.Validation("WorkOrder.ReasonRequired", "El motivo de observación es requerido.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Observado;
        ObservationNotes = reason.Trim();
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Observado,
            reason,
            updatedByUserId));

        return Result.Success;
    }

    public ErrorOr<Success> ResolveObservation(string resolutionNotes, Guid updatedByUserId)
    {
        if (Status != WorkOrderStatus.Observado)
        {
            return Error.Validation("WorkOrder.NotInObservedStatus", "La orden no se encuentra en estado Observado.");
        }

        if (string.IsNullOrWhiteSpace(resolutionNotes))
        {
            return Error.Validation("WorkOrder.ResolutionNotesRequired", "La nota de subsanación es requerida.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Pendiente;
        ObservationNotes = null;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Pendiente,
            $"Observación subsanada: {resolutionNotes.Trim()}",
            updatedByUserId));

        return Result.Success;
    }

    public ErrorOr<Success> Reject(string reason, Guid updatedByUserId)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Error.Validation("WorkOrder.ReasonRequired", "El motivo de rechazo es requerido.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Rechazado;
        RejectionReason = reason.Trim();
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Rechazado,
            reason,
            updatedByUserId));

        return Result.Success;
    }

    public ErrorOr<Success> Complete(string? completionNotes, Guid updatedByUserId)
    {
        if (Status != WorkOrderStatus.Agendado && Status != WorkOrderStatus.Pendiente)
        {
            return Error.Validation("WorkOrder.InvalidStateForCompletion", "Solo se puede completar una solicitud agendada o pendiente.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Completado;
        CompletedAt = DateTimeOffset.UtcNow;
        CompletionNotes = completionNotes?.Trim();
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Completado,
            completionNotes ?? "Atención completada y verificada por Backoffice.",
            updatedByUserId));

        return Result.Success;
    }

    public ErrorOr<Success> Cancel(string? reason, Guid updatedByUserId)
    {
        if (Status == WorkOrderStatus.Completado)
        {
            return Error.Validation("WorkOrder.InvalidStateForCancel", "No se puede cancelar una orden que ya ha sido completada.");
        }

        var oldStatus = Status;
        Status = WorkOrderStatus.Cancelado;
        CancellationReason = reason?.Trim();
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = updatedByUserId;

        _statusHistory.Add(WorkOrderStatusHistory.Create(
            Id,
            oldStatus,
            WorkOrderStatus.Cancelado,
            reason ?? "Solicitud cancelada.",
            updatedByUserId));

        return Result.Success;
    }
}
