using System.ComponentModel;

using Skvia.BaseTemplate.Application.Common.Attributes;

namespace Skvia.BaseTemplate.Application.Common.Security;


/// <summary>
/// Definiciones de permisos del sistema para el módulo de empleados.
/// </summary>
public static partial class Permission
{
    /// <summary>
    /// Permisos específicos para la gestión y administración de empleados.
    /// </summary>
    [DisplayName("Permisos Empleados")]
    [Description("Establece los permisos para las operaciones con empleados.")]
    public static class Employee
    {
        /// <summary>
        /// Permiso para consultar y visualizar el listado y detalle de empleados.
        /// </summary>
        [PermissionInfo("Ver Empleado", "Permite ver la lista de empleados.")]
        public const string View = "Permissions.Employees.View";

        /// <summary>
        /// Permiso para registrar un nuevo empleado.
        /// </summary>
        [PermissionInfo("Crear Empleado", "Permite crear un empleado.")]
        public const string Create = "Permissions.Employees.Create";

        /// <summary>
        /// Permiso para actualizar la información de empleados.
        /// </summary>
        [PermissionInfo("Editar Empleado", "Permite editar un empleado.")]
        public const string Update = "Permissions.Employees.Update";

        /// <summary>
        /// Permiso para eliminar empleados.
        /// </summary>
        [PermissionInfo("Eliminar Empleado", "Permite eliminar un empleado.")]
        public const string Delete = "Permissions.Employees.Delete";

        /// <summary>
        /// Permiso para archivar o desarchivar fichas de empleados.
        /// </summary>
        [PermissionInfo("Archivar Empleado", "Permite archivar o desarchivar un empleado.")]
        public const string Archive = "Permissions.Employees.Archive";
    }
}

