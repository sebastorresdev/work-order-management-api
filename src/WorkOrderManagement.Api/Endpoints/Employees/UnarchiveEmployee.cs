using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Application.Features.Employees.Commands.UnarchiveEmployee;

namespace WorkOrderManagement.Api.Endpoints.Employees;

public sealed class UnarchiveEmployee : IEndpoint
{
    public static void Map(RouteGroupBuilder group)
        => group.MapPatch("/{id:guid}/unarchive", Handle)
            .WithName(nameof(UnarchiveEmployee))
            .WithSummary("Desarchivar empleado")
            .WithDescription("Desarchiva un empleado previamente archivado en el sistema.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound);

    private static async Task<IResult> Handle(
        Guid id,
        ICommandHandler<UnarchiveEmployeeCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var command = new UnarchiveEmployeeCommand(id);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.Match(
            _ => TypedResults.NoContent(),
            errors => errors.ToProblem());
    }
}
