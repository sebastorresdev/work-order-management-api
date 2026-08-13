using System.ComponentModel;
using WorkOrderManagement.Application.Common.Attributes;

namespace WorkOrderManagement.Application.Common.Security;

public static partial class Permission
{
    /// <summary>
    /// Permisos específicos para la gestión de solicitudes de servicio y órdenes de trabajo.
    /// </summary>
    [DisplayName("Permisos Solicitudes de Servicio")]
    [Description("Establece los permisos para las operaciones de solicitudes de servicio / órdenes de trabajo")]
    public static class WorkOrders
    {
        [PermissionInfo("Ver Solicitudes", "Permite consultar y visualizar las solicitudes de servicio")]
        public const string View = "Permissions.WorkOrders.View";

        [PermissionInfo("Crear Solicitud", "Permite registrar una nueva solicitud de servicio")]
        public const string Create = "Permissions.WorkOrders.Create";

        [PermissionInfo("Editar Solicitud", "Permite modificar los datos de una solicitud de servicio")]
        public const string Edit = "Permissions.WorkOrders.Edit";

        [PermissionInfo("Agendar Solicitud", "Permite agendar o reprogramar la atención técnica de una solicitud")]
        public const string Schedule = "Permissions.WorkOrders.Schedule";

        [PermissionInfo("Completar Solicitud", "Permite marcar una solicitud como completada y atendida")]
        public const string Complete = "Permissions.WorkOrders.Complete";

        [PermissionInfo("Cancelar Solicitud", "Permite observar, rechazar o cancelar una solicitud")]
        public const string Cancel = "Permissions.WorkOrders.Cancel";
    }
}
