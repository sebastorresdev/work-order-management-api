using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using WorkOrderManagement.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using WorkOrderManagement.Application.Features.Notifications.DTOs;
using WorkOrderManagement.Application.Features.Notifications.Queries.GetNotifications;

namespace WorkOrderManagement.Api.Endpoints.Notifications;

/// <summary>
/// Mapeo de endpoints de la API HTTP para la gestión de notificaciones del usuario.
/// </summary>
public sealed class NotificationEndpoints : IEndpoint
{
    /// <summary>
    /// Registra las rutas asociadas a notificaciones en el enrutador de ASP.NET Core.
    /// </summary>
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", GetNotifications)
            .WithName("GetNotifications")
            .WithSummary("Obtener notificaciones del usuario autenticado")
            .Produces<List<NotificationResponse>>(StatusCodes.Status200OK);

        group.MapPut("/{id:guid}/read", MarkNotificationAsRead)
            .WithName("MarkNotificationAsRead")
            .WithSummary("Marcar notificación como leída")
            .Produces(StatusCodes.Status204NoContent);

        group.MapPut("/read-all", MarkAllNotificationsAsRead)
            .WithName("MarkAllNotificationsAsRead")
            .WithSummary("Marcar todas las notificaciones como leídas")
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> GetNotifications(
        ICurrentUserProvider currentUserProvider,
        IQueryHandler<GetNotificationsQuery, ErrorOr<List<NotificationResponse>>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var query = new GetNotificationsQuery(currentUser.Id);
        var result = await handler.HandleAsync(query, cancellationToken);
        return result.Match(TypedResults.Ok, errors => errors.ToProblem());
    }

    private static async Task<IResult> MarkNotificationAsRead(
        Guid id,
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<MarkNotificationAsReadCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new MarkNotificationAsReadCommand(id, currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }

    private static async Task<IResult> MarkAllNotificationsAsRead(
        ICurrentUserProvider currentUserProvider,
        ICommandHandler<MarkAllNotificationsAsReadCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var command = new MarkAllNotificationsAsReadCommand(currentUser.Id);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.Match(_ => TypedResults.NoContent(), errors => errors.ToProblem());
    }
}
