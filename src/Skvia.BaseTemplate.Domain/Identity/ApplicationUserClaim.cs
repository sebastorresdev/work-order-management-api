using Microsoft.AspNetCore.Identity;

namespace Skvia.BaseTemplate.Domain.Identity;

/// <summary>
/// Representa un claim o permiso directo asignado a un usuario específico.
/// </summary>
public class ApplicationUserClaim : IdentityUserClaim<Guid>
{
    /// <summary>
    /// Descripción textual del propósito del claim del usuario.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Entidad de navegación hacia el usuario al que se le asignó este claim.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = default!;
}

