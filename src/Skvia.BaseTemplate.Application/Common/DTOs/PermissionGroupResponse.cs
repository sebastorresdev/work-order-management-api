namespace Skvia.BaseTemplate.Application.Common.DTOs;

/// <summary>
/// DTO de respuesta que agrupa un conjunto de permisos bajo una categoría o módulo común.
/// </summary>
/// <param name="Group">Nombre del grupo de permisos.</param>
/// <param name="GroupDescription">Descripción clara sobre la categoría de permisos.</param>
/// <param name="Permissions">Lista de elementos de permisos individuales pertenecientes a este grupo.</param>
public record PermissionGroupResponse(
    string Group,
    string GroupDescription,
    List<PermissionItemResponse> Permissions
);

