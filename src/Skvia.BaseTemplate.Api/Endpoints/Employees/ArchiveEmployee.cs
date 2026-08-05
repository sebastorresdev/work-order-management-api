using Skvia.BaseTemplate.Api.Models;
using Skvia.BaseTemplate.Application.Features.Employees.Commands.ArchiveEmployee;

namespace Skvia.BaseTemplate.Api.Endpoints.Employees;

public sealed class ArchiveEmployee : IEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/archive", Handle)
            .WithName(nameof(ArchiveEmployee))
            .WithSummary("Archivar empleado")
            .WithDescription("Archiva un empleado en el sistema.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ApiProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(
        Guid id,
        ICommandHandler<ArchiveEmployeeCommand, ErrorOr<Success>> handler,
        CancellationToken cancellationToken)
    {
        var command = new ArchiveEmployeeCommand(id);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.Match(
            _ => TypedResults.NoContent(),
            errors => errors.ToProblem());
    }
}
