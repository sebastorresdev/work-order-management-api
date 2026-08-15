using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.CancelWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.CompleteWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.CreateWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.ObserveWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.RejectWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.ScheduleWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.Commands.UpdateWorkOrder;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrderById;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Api.Endpoints.WorkOrders;

public sealed class WorkOrderEndpoints : IEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", GetWorkOrders)
            .WithName("GetWorkOrders")
            .WithSummary("Obtener lista de órdenes de trabajo paginada")
            .Produces<PaginatedResponse<WorkOrderResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetWorkOrderById)
            .WithName("GetWorkOrderById")
            .WithSummary("Obtener detalle completo de una orden de trabajo")
            .Produces<WorkOrderDetailResponse>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateWorkOrder)
            .WithName("CreateWorkOrder")
            .WithSummary("Crear nueva solicitud de servicio / orden de trabajo")
            .Produces<WorkOrderResponse>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", UpdateWorkOrder)
            .WithName("UpdateWorkOrder")
            .WithSummary("Actualizar datos de una orden de trabajo (Pendiente u Observada)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/schedule", ScheduleWorkOrder)
            .WithName("ScheduleWorkOrder")
            .WithSummary("Agendar o reprogramar atención para una orden (Backoffice)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/observe", ObserveWorkOrder)
            .WithName("ObserveWorkOrder")
            .WithSummary("Observar orden de trabajo para corrección (Backoffice)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/reject", RejectWorkOrder)
            .WithName("RejectWorkOrder")
            .WithSummary("Rechazar orden de trabajo (Backoffice)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/complete", CompleteWorkOrder)
            .WithName("CompleteWorkOrder")
            .WithSummary("Marcar orden de trabajo como completada (Backoffice)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/cancel", CancelWorkOrder)
            .WithName("CancelWorkOrder")
            .WithSummary("Cancelar orden de trabajo (Vendedor / Backoffice)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/resolve-observation", ResolveObservation)
            .WithName("ResolveObservation")
            .WithSummary("Subsanar/Responder observación de una orden de trabajo (Vendedor)")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetWorkOrders(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string? searchTerm,
        [FromQuery] WorkOrderStatus? status,
        [FromQuery] WorkOrderType? requestType,
        [FromQuery] Guid? branchId,
        ClaimsPrincipal userClaims,
        ICurrentUserProvider currentUserProvider,
        IQueryHandler<GetWorkOrdersQuery, ErrorOr<PaginatedResponse<WorkOrderResponse>>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var userRoles = userClaims.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct()
            .ToList();

        var query = new GetWorkOrdersQuery(
            pageNumber < 1 ? 1 : pageNumber,
            pageSize < 1 ? 10 : pageSize,
            searchTerm,
            status,
            requestType,
            branchId,
            currentUser.Id,
            userRoles);

        var result = await handler.HandleAsync(query, cancellationToken);
        return result.Match(TypedResults.Ok, errors => errors.ToProblem());
    }

    private static async Task<IResult> GetWorkOrderById(
        Guid id,
        IQueryHandler<GetWorkOrderByIdQuery, ErrorOr<WorkOrderDetailResponse>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetWorkOrderByIdQuery(id);
        var result = await handler.HandleAsync(query, cancellationToken);
        return result.Match(TypedResults.Ok, errors => errors.ToProblem());
    }

    private static async Task<IResult> CreateWorkOrder(
        CreateWorkOrderApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<CreateWorkOrderCommand, ErrorOr<WorkOrderResponse>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new CreateWorkOrderCommand(
            request.RequestType,
            request.Priority,
            request.BranchId,
            currentUser.Id,
            request.ClientCode,
            request.ClientName,
            request.ClientPhone,
            request.ClientSecondaryPhone,
            request.District,
            request.Address,
            request.AddressReference,
            request.Description);

        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(
            workOrder => TypedResults.Created($"/api/v1/work-orders/{workOrder.Id}", workOrder),
            errors => errors.ToProblem());
    }

    private static async Task<IResult> UpdateWorkOrder(
        Guid id,
        UpdateWorkOrderApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<UpdateWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new UpdateWorkOrderCommand(
            id,
            request.RequestType,
            request.Priority,
            request.ClientCode,
            request.ClientName,
            request.ClientPhone,
            request.ClientSecondaryPhone,
            request.District,
            request.Address,
            request.AddressReference,
            request.Description,
            currentUser.Id);

        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> ScheduleWorkOrder(
        Guid id,
        ScheduleWorkOrderApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<ScheduleWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new ScheduleWorkOrderCommand(
            id,
            request.ScheduledDate,
            request.ScheduledSlot,
            request.AssignedTechnicianId,
            request.Notes,
            currentUser.Id);

        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> ObserveWorkOrder(
        Guid id,
        ReasonApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<ObserveWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new ObserveWorkOrderCommand(id, request.Reason, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> RejectWorkOrder(
        Guid id,
        ReasonApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<RejectWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new RejectWorkOrderCommand(id, request.Reason, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> CompleteWorkOrder(
        Guid id,
        CompleteWorkOrderApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<CompleteWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new CompleteWorkOrderCommand(id, request.CompletionNotes, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> CancelWorkOrder(
        Guid id,
        ReasonApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<CancelWorkOrderCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new CancelWorkOrderCommand(id, request.Reason, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> ResolveObservation(
        Guid id,
        ResolveObservationApiRequest request,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation.ResolveObservationCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation.ResolveObservationCommand(id, request.ResolutionNotes, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }
}

public record CreateWorkOrderApiRequest(
    WorkOrderType RequestType,
    WorkOrderPriority Priority,
    Guid BranchId,
    string ClientCode,
    string ClientName,
    string ClientPhone,
    string? ClientSecondaryPhone,
    string District,
    string Address,
    string? AddressReference,
    string Description);

public record UpdateWorkOrderApiRequest(
    WorkOrderType RequestType,
    WorkOrderPriority Priority,
    string ClientCode,
    string ClientName,
    string ClientPhone,
    string? ClientSecondaryPhone,
    string District,
    string Address,
    string? AddressReference,
    string Description);

public record ScheduleWorkOrderApiRequest(
    DateOnly ScheduledDate,
    string ScheduledSlot,
    Guid? AssignedTechnicianId,
    string? Notes);

public record ReasonApiRequest(string Reason);

public record CompleteWorkOrderApiRequest(string? CompletionNotes);

public record ResolveObservationApiRequest(string ResolutionNotes);
