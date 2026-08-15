using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Users.DTOs;
using WorkOrderManagement.Application.Features.Users.Queries.GetTechnicians;

namespace WorkOrderManagement.Api.Endpoints.Users;

public sealed class GetTechnicians : IEndpoint
{
    public static void Map(RouteGroupBuilder group)
        => group.MapGet("/technicians", Handle)
            .WithName(nameof(GetTechnicians))
            .WithSummary("Obtener técnicos")
            .WithDescription("Obtiene el listado de usuarios con el rol Técnico, opcionalmente filtrados por sede.")
            .Produces<List<UserResponse>>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

    private static async Task<IResult> Handle(
        [FromQuery] Guid? branchId,
        IQueryHandler<GetTechniciansQuery, ErrorOr<List<UserResponse>>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetTechniciansQuery(branchId);
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.Match(
            TypedResults.Ok,
            errors => errors.ToProblem());
    }
}
