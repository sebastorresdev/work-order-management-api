namespace WorkOrderManagement.Application.Common.Security.Roles;

/// <summary>
/// Contiene constantes con los nombres estándar de roles del sistema.
/// </summary>
public static class Roles
{
    /// <summary>
    /// Rol de Administrador con acceso global a la configuración y gestión.
    /// </summary>
    public const string Administrator = "Administrator";

    /// <summary>
    /// Rol estándar de Empleado.
    /// </summary>
    public const string Employee = "Empleado";

    /// <summary>
    /// Rol de Vendedor para operaciones comerciales y registro de solicitudes.
    /// </summary>
    public const string Sales = "Vendedor";

    /// <summary>
    /// Rol de Backoffice para gestión, derivación y agendamiento de trabajos.
    /// </summary>
    public const string Backoffice = "Backoffice";

    /// <summary>
    /// Rol de Supervisor para supervisar equipos de vendedores y trabajos de la sede.
    /// </summary>
    public const string Supervisor = "Supervisor";

    /// <summary>
    /// Rol de Técnico de campo para atención de solicitudes de servicio.
    /// </summary>
    public const string Technician = "Técnico";
}

