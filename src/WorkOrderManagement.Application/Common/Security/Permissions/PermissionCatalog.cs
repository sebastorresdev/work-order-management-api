using System.ComponentModel;
using System.Reflection;

using WorkOrderManagement.Application.Common.Attributes;

namespace WorkOrderManagement.Application.Common.Security.Permissions;

/// <summary>
/// Catálogo encargado de inspeccionar vía reflexión las clases de permisos para construir el árbol completo de permisos del sistema.
/// </summary>
public static class PermissionCatalog
{
    /// <summary>
    /// Escanea las definiciones estáticas de permisos y sus atributos para retornar la lista de grupos y elementos de permisos.
    /// </summary>
    /// <returns>Lista de grupos de permisos registrados con sus respectivos metadatos.</returns>
    public static List<PermissionCatalogGroupResponse> GetAll()
    {
        // Lista resultante de grupos de permisos
        var permissions = new List<PermissionCatalogGroupResponse>();

        // Obtención de las clases anidadas dentro de Permission
        var groups = typeof(Permission).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var group in groups)
        {
            // Nombre visible del grupo extraído del atributo DisplayName
            var groupDisplay = group.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                               ?? group.Name;

            // Descripción del grupo extraída del atributo Description
            var groupDescription = group.GetCustomAttribute<DescriptionAttribute>()?.Description
                                  ?? string.Empty;

            var permissionsItem = new List<PermissionCatalogItemResponse>();

            // Obtención de las constantes públicas del grupo
            var fields = group.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<PermissionInfoAttribute>();
                if (attr == null)
                    continue;

                permissionsItem.Add(new PermissionCatalogItemResponse(
                    Key: field.GetValue(null)?.ToString() ?? "",
                    Display: attr.Display,
                    Description: attr.Description
                ));
            }

            permissions.Add(new PermissionCatalogGroupResponse(
                Group: groupDisplay,
                GroupDescription: groupDescription,
                Permissions: permissionsItem
            ));
        }

        return permissions;
    }
}

