using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Domain.Branches;

/// <summary>
/// Representa la relación muchos a muchos entre una sede (Branch) y un usuario (ApplicationUser).
/// </summary>
public class BranchUser : BaseAuditableEntity
{
    /// <summary>
    /// Identificador único de la sede vinculada.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Entidad de navegación hacia la sede relacionada.
    /// </summary>
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// Identificador único del usuario asignado a la sede.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Entidad de navegación hacia el usuario de la aplicación.
    /// </summary>
    public ApplicationUser User { get; set; } = null!;
}

