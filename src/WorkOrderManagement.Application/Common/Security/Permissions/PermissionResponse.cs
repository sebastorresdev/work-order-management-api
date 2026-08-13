namespace WorkOrderManagement.Application.Common.Security.Permissions;

/// <summary>
/// DTO de respuesta que representa la información de catálogo de un permiso individual.
/// </summary>
/// <param name="Key">Clave o código único del permiso.</param>
/// <param name="Display">Nombre de presentación amigable.</param>
/// <param name="Description">Descripción explicativa del permiso.</param>
public record PermissionCatalogItemResponse(
    string Key,
    string Display,
    string Description
);

/// <summary>
/// DTO de respuesta que representa un grupo de permisos en el catálogo del sistema.
/// </summary>
/// <param name="Group">Nombre del grupo.</param>
/// <param name="GroupDescription">Descripción del grupo.</param>
/// <param name="Permissions">Permisos contenidos en el grupo.</param>
public record PermissionCatalogGroupResponse(
    string Group,
    string GroupDescription,
    List<PermissionCatalogItemResponse> Permissions
);

