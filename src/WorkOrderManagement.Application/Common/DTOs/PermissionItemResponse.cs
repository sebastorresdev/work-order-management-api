namespace WorkOrderManagement.Application.Common.DTOs;

/// <summary>
/// DTO de respuesta que representa un permiso individual, su estado de asignación y su fuente.
/// </summary>
/// <param name="Key">Clave única del permiso (por ejemplo: "Permissions.Users.View").</param>
/// <param name="Display">Nombre legible para la interfaz de usuario.</param>
/// <param name="Description">Descripción explicativa de la acción permitida.</param>
/// <param name="Granted">Indica si el permiso ha sido otorgado.</param>
/// <param name="Source">Origen de la asignación del permiso (por ejemplo: "Role", "UserOverride", null).</param>
public record PermissionItemResponse(
    string Key,
    string Display,
    string Description,
    bool Granted,
    string? Source
);

