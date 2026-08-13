using Microsoft.AspNetCore.Identity;

namespace WorkOrderManagement.Domain.Identity;

/// <summary>
/// Representa un claim o permiso asociado a un rol del sistema.
/// </summary>
public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    /// <summary>
    /// Descripción textual aclaratoria sobre el permiso o claim.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Grupo o categoría funcional a la que pertenece este permiso (por ejemplo: "Gestión de Usuarios").
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Entidad de navegación hacia el rol poseedor de este claim.
    /// </summary>
    public virtual ApplicationRole Role { get; set; } = default!;
}

