using Skvia.BaseTemplate.Api.Endpoints.Users.Requests;
using Skvia.BaseTemplate.Api.Models;
using Skvia.BaseTemplate.Application.Features.Users.Commands.ToggleUserStatus;

namespace Skvia.BaseTemplate.Api.Endpoints.Users;

public sealed class ToggleUserStatus : IEndpoint
{
    public static void Map(RouteGroupBuilder group)
        => group.MapPatch("/{userId:guid}/status", Handle)
            .WithName(nameof(ToggleUserStatus))
            .WithSummary("Cambiar estado de usuario")
            .WithDescription("Activa o desactiva un usuario.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound);

    private static async Task<IResult> Handle(
        Guid userId,
        ToggleUserStatusRequest request,
        ICommandHandler<ToggleUserStatusCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var command = new ToggleUserStatusCommand(userId, request.IsActive);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.Match(
            _ => TypedResults.NoContent(),
            errors => errors.ToProblem());
    }
}

