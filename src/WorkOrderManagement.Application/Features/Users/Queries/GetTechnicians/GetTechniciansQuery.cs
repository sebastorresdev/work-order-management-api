using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetTechnicians;

/// <summary>
/// Consulta CQRS para obtener la lista de usuarios con el rol 'Técnico', opcionalmente filtrados por sede.
/// </summary>
/// <param name="BranchId">Identificador opcional de la sede para filtrar la búsqueda.</param>
public record GetTechniciansQuery(Guid? BranchId = null) : IQuery<ErrorOr<List<UserResponse>>>;
