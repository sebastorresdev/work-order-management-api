using Microsoft.AspNetCore.Identity;

namespace Skvia.BaseTemplate.Domain.Identity;

/// <summary>
/// Entidad de dominio que extiende la clase <see cref="IdentityRole{Guid}"/> de ASP.NET Core Identity para personalizar los roles del sistema.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Constructor por defecto que inicializa las colecciones de claims y roles de usuario asociados.
    /// </summary>
    public ApplicationRole()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        UserRoles = new HashSet<ApplicationUserRole>();
    }

    /// <summary>
    /// Constructor que asigna el nombre del rol e inicializa las colecciones asociadas.
    /// </summary>
    /// <param name="roleName">Nombre del rol a crear.</param>
    public ApplicationRole(string roleName) : base(roleName)
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        UserRoles = new HashSet<ApplicationUserRole>();
    }

    /// <summary>
    /// Descripción detallada del propósito o alcance del rol.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Colección de permisos/claims explícitamente asignados a este rol.
    /// </summary>
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

    /// <summary>
    /// Colección de asignaciones de usuarios que poseen este rol.
    /// </summary>
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    /// <summary>
    /// Fecha y hora de creación del rol.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación del rol.
    /// </summary>
    public DateTime LastModifiedAt { get; set; }
}

