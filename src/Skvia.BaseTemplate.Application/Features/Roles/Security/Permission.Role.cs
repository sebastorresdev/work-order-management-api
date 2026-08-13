using System.ComponentModel;

using Skvia.BaseTemplate.Application.Common.Attributes;

namespace Skvia.BaseTemplate.Application.Common.Security;

/// <summary>
/// Definiciones de permisos del sistema para el módulo de roles.
/// </summary>
public static partial class Permission
{
    /// <summary>
    /// Permisos específicos para la gestión y administración de roles.
    /// </summary>
    [DisplayName("Permisos de Roles")]
    [Description("Define los permisos para la administración de roles")]
    public static class Role
    {
        /// <summary>
        /// Permiso para ver el catálogo y detalle de roles.
        /// </summary>
        [PermissionInfo("Ver Roles", "Permite ver la lista de roles")]
        public const string View = "Permissions.Roles.View";

        /// <summary>
        /// Permiso para crear nuevos roles de usuario.
        /// </summary>
        [PermissionInfo("Crear Rol", "Permite crear nuevos roles")]
        public const string Create = "Permissions.Roles.Create";

        /// <summary>
        /// Permiso para editar roles existentes y sus asignaciones.
        /// </summary>
        [PermissionInfo("Editar Rol", "Permite editar roles existentes")]
        public const string Edit = "Permissions.Roles.Edit";

        /// <summary>
        /// Permiso para eliminar roles.
        /// </summary>
        [PermissionInfo("Eliminar Rol", "Permite eliminar roles")]
        public const string Delete = "Permissions.Roles.Delete";
    }
}

