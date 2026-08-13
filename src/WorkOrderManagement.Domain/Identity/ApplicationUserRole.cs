using Microsoft.AspNetCore.Identity;

namespace WorkOrderManagement.Domain.Identity;
/// <summary>
/// Representa la tabla de unión entre usuarios y roles (relación muchos a muchos).
/// </summary>
public class ApplicationUserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// Entidad de navegación hacia el usuario.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = default!;

    /// <summary>
    /// Entidad de navegación hacia el rol asignado.
    /// </summary>
    public virtual ApplicationRole Role { get; set; } = default!;
}

