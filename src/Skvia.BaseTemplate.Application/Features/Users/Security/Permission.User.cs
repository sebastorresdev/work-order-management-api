using System.ComponentModel;

using Skvia.BaseTemplate.Application.Common.Attributes;

namespace Skvia.BaseTemplate.Application.Common.Security;

/// <summary>
/// Definiciones de permisos del sistema para el módulo de usuarios.
/// </summary>
public static partial class Permission
{
    /// <summary>
    /// Permisos específicos para la gestión y administración de usuarios.
    /// </summary>
    [DisplayName("Permisos de Usuarios")]
    [Description("Define los permisos para la administración de usuarios")]
    public static class User
    {
        /// <summary>
        /// Permiso para ver el listado y detalle de usuarios del sistema.
        /// </summary>
        [PermissionInfo("Ver Usuarios", "Permite ver la lista de usuarios")]
        public const string View = "Permissions.Users.View";

        /// <summary>
        /// Permiso para registrar y crear nuevos usuarios.
        /// </summary>
        [PermissionInfo("Crear Usuario", "Permite crear nuevos usuarios")]
        public const string Create = "Permissions.Users.Create";

        /// <summary>
        /// Permiso para editar información de usuarios existentes.
        /// </summary>
        [PermissionInfo("Editar Usuario", "Permite editar usuarios existentes")]
        public const string Edit = "Permissions.Users.Edit";

        /// <summary>
        /// Permiso para eliminar cuentas de usuario.
        /// </summary>
        [PermissionInfo("Eliminar Usuario", "Permite eliminar usuarios")]
        public const string Delete = "Permissions.Users.Delete";

        /// <summary>
        /// Permiso para archivar o desarchivar usuarios.
        /// </summary>
        [PermissionInfo("Archivar Usuario", "Permite archivar o desarchivar usuarios")]
        public const string Archive = "Permissions.Users.Archive";
    }
}

