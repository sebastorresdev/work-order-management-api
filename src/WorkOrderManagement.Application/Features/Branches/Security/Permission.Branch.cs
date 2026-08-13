using System.ComponentModel;

using WorkOrderManagement.Application.Common.Attributes;

namespace WorkOrderManagement.Application.Common.Security;

/// <summary>
/// Contiene las definiciones parciales de constantes de permisos del sistema.
/// </summary>
public static partial class Permission
{
    /// <summary>
    /// Permisos específicos para la gestión y administración de sedes/sucursales.
    /// </summary>
    [DisplayName("Permisos Sedes")]
    [Description("Establece los permisos para las operaciones con sedes")]
    public static class Branch
    {
        /// <summary>
        /// Permiso para la creación de nuevas sedes.
        /// </summary>
        [PermissionInfo("Crear Sucursal", "Permite crear una sucursal")]
        public const string Create = "Permissions.Branches.Create";

        /// <summary>
        /// Permiso para modificar los datos de sedes existentes.
        /// </summary>
        [PermissionInfo("Actualizar Sucursal", "Permite actualizar una sucursal")]
        public const string Update = "Permissions.Branches.Update";

        /// <summary>
        /// Permiso para eliminar sedes del sistema.
        /// </summary>
        [PermissionInfo("Eliminar Sucursal", "Permite eliminar una sucursal")]
        public const string Delete = "Permissions.Branches.Delete";

        /// <summary>
        /// Permiso para consultar y visualizar el listado de sedes.
        /// </summary>
        [PermissionInfo("Ver Sucursales", "Permite ver la lista de sucursales")]
        public const string View = "Permissions.Branches.View";

        /// <summary>
        /// Permiso para archivar o desarchivar sedes.
        /// </summary>
        [PermissionInfo("Archivar Sucursal", "Permite archivar o desarchivar una sucursal")]
        public const string Archive = "Permissions.Branches.Archive";
    }
}

