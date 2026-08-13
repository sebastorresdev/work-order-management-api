using Microsoft.AspNetCore.Identity;

using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Domain.Identity;

/// <summary>
/// Entidad de usuario principal de la aplicación que extiende <see cref="IdentityUser{Guid}"/> con propiedades de dominio personalizadas.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Constructor por defecto que inicializa las colecciones relacionadas de claims, roles, logins, tokens y sedes asociadas.
    /// </summary>
    public ApplicationUser()
    {
        UserClaims = new HashSet<ApplicationUserClaim>();
        UserRoles = new HashSet<ApplicationUserRole>();
        Logins = new HashSet<ApplicationUserLogin>();
        Tokens = new HashSet<ApplicationUserToken>();
        BranchUsers = new HashSet<BranchUser>();
        Subordinates = new HashSet<ApplicationUser>();
    }

    /// <summary>
    /// Identificador opcional de la sede principal asignada al usuario.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Entidad de navegación hacia la sede principal asociada al usuario.
    /// </summary>
    public virtual Branch? Branch { get; set; }

    /// <summary>
    /// Identificador del usuario supervisor a cargo.
    /// </summary>
    public Guid? SupervisorId { get; set; }

    /// <summary>
    /// Entidad de navegación hacia el usuario supervisor.
    /// </summary>
    public virtual ApplicationUser? Supervisor { get; set; }

    /// <summary>
    /// Colección de usuarios subordinados a cargo de este supervisor.
    /// </summary>
    public virtual ICollection<ApplicationUser> Subordinates { get; set; }

    /// <summary>
    /// Colección de claims o permisos específicos asignados a nivel de usuario.
    /// </summary>
    public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }

    /// <summary>
    /// Colección de roles asignados a este usuario.
    /// </summary>
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    /// <summary>
    /// Colección de inicios de sesión externos del usuario.
    /// </summary>
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; }

    /// <summary>
    /// Colección de tokens asociados al usuario.
    /// </summary>
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

    /// <summary>
    /// Colección de relaciones entre el usuario y las múltiples sedes asignadas.
    /// </summary>
    public ICollection<BranchUser> BranchUsers { get; set; }

    /// <summary>
    /// Nombre visible o de presentación del usuario.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Indica si el usuario está actualmente activo en el sistema.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indica si la cuenta del usuario se encuentra archivada o deshabilitada.
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// URL de la foto de perfil del usuario.
    /// </summary>
    public string? ProfilePhotoUrl { get; set; }

    /// <summary>
    /// Token de refresco JWT para renovación de sesiones.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Fecha y hora de expiración del token de refresco en UTC.
    /// </summary>
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// Fecha y hora de creación de la cuenta de usuario.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación en los datos del usuario.
    /// </summary>
    public DateTime LastModifiedAt { get; set; }
}
