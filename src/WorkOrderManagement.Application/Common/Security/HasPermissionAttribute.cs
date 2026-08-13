namespace WorkOrderManagement.Application.Common.Security;

/// <summary>
/// Atributo para anotar clases de comandos o consultas que requieren un permiso específico para su ejecución.
/// </summary>
/// <param name="permission">Cadena con el nombre único del permiso exigido (por ejemplo: "Permissions.Users.Create").</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class HasPermissionAttribute(string permission) : Attribute
{
    /// <summary>
    /// Permiso requerido para poder ejecutar la solicitud decorada.
    /// </summary>
    public string Permission { get; } = permission;
}
