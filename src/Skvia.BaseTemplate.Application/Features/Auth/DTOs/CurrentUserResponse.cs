namespace Skvia.BaseTemplate.Application.Features.Auth.DTOs;

/// <summary>
/// DTO de respuesta que contiene el contexto y permisos del usuario actualmente autenticado.
/// </summary>
/// <param name="Id">Identificador único del usuario.</param>
/// <param name="Roles">Lista de roles asignados al usuario.</param>
/// <param name="Permissions">Lista de claves de permisos otorgados al usuario.</param>
public record CurrentUserResponse(
    Guid Id,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions);

