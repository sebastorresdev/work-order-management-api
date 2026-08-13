namespace WorkOrderManagement.Application.Features.Users.DTOs;

/// <summary>
/// DTO de respuesta que contiene la información resumida de un usuario para listados.
/// </summary>
/// <param name="Id">Identificador único del usuario.</param>
/// <param name="UserName">Nombre de usuario para iniciar sesión.</param>
/// <param name="IsActive">Estado activo o inactivo de la cuenta.</param>
/// <param name="BranchName">Nombre de la sede asignada.</param>
/// <param name="RoleNames">Lista de nombres de los roles asignados.</param>
/// <param name="Email">Correo electrónico del usuario.</param>
/// <param name="PhotoUrl">URL de la foto de perfil.</param>
/// <param name="LastModifiedAt">Fecha y hora de la última modificación.</param>
public record UserResponse(
    Guid Id,
    string UserName,
    bool IsActive,
    string BranchName,
    List<string> RoleNames,
    string? Email,
    string? PhotoUrl,
    DateTime LastModifiedAt
);

