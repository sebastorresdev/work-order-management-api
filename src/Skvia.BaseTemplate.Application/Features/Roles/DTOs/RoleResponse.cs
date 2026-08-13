namespace Skvia.BaseTemplate.Application.Features.Roles.DTOs;

/// <summary>
/// DTO de respuesta con los datos de un rol.
/// </summary>
/// <param name="Id">Identificador único del rol.</param>
/// <param name="Name">Nombre del rol.</param>
/// <param name="Description">Descripción opcional del rol.</param>
/// <param name="LastModifiedAt">Fecha y hora de la última modificación.</param>
public record RoleResponse(Guid Id, string Name, string? Description, DateTime LastModifiedAt);

