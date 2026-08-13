using Microsoft.AspNetCore.Identity;

namespace WorkOrderManagement.Domain.Identity;

/// <summary>
/// Representa una autenticación de usuario realizada mediante un proveedor externo (por ejemplo, Google, Azure AD).
/// </summary>
public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    /// <summary>
    /// Entidad de navegación hacia el usuario asociado con esta autenticación externa.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = default!;
}

