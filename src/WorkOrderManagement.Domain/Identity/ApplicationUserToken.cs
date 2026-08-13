using Microsoft.AspNetCore.Identity;

namespace WorkOrderManagement.Domain.Identity;

/// <summary>
/// Representa un token de autenticación o verificación generado para un usuario.
/// </summary>
public class ApplicationUserToken : IdentityUserToken<Guid>
{
    /// <summary>
    /// Entidad de navegación hacia el usuario titular del token.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = default!;
}

