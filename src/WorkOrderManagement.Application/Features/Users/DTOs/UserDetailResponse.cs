namespace WorkOrderManagement.Application.Features.Users.DTOs;

/// <summary>
/// DTO de respuesta que contiene el detalle completo de un usuario para formularios de edición y consulta profunda.
/// </summary>
/// <param name="Id">Identificador único del usuario.</param>
/// <param name="DisplayName">Nombre de presentación del usuario.</param>
/// <param name="UserName">Nombre de usuario.</param>
/// <param name="IsActive">Estado de la cuenta (activa/inactiva).</param>
/// <param name="BranchIds">Lista de identificadores de las sedes asignadas al usuario.</param>
/// <param name="RoleIds">Lista de identificadores de los roles asignados al usuario.</param>
/// <param name="Email">Correo electrónico del usuario.</param>
/// <param name="PhotoUrl">URL de la foto de perfil.</param>
/// <param name="PhoneNumber">Número de teléfono de contacto.</param>
/// <param name="CreatedAt">Fecha y hora de creación de la cuenta.</param>
/// <param name="LastModifiedAt">Fecha y hora de la última modificación.</param>
public record UserDetailResponse(
    Guid Id,
    string? DisplayName,
    string UserName,
    bool IsActive,
    List<Guid> BranchIds,
    List<Guid> RoleIds,
    string? Email,
    string? PhotoUrl,
    string? PhoneNumber,
    DateTime CreatedAt,
    DateTime LastModifiedAt
);

