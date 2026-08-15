using ErrorOr;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetTechnicians;

/// <summary>
/// Manejador de la consulta CQRS para listar los técnicos disponibles en el sistema o en una sede específica.
/// </summary>
public class GetTechniciansQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetTechniciansQuery, ErrorOr<List<UserResponse>>>
{
    /// <summary>
    /// Ejecuta la consulta de filtrado de usuarios con el rol 'Técnico'.
    /// </summary>
    public Task<ErrorOr<List<UserResponse>>> HandleAsync(GetTechniciansQuery query, CancellationToken cancellationToken)
        => userAccountService.GetTechniciansAsync(query.BranchId, cancellationToken);
}
