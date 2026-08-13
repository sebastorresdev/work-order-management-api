using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

/// <summary>
/// Servicio para la resolución y evaluación de permisos efectivos asociados a un usuario.
/// </summary>
public interface IUserPermissionService
{
    /// <summary>
    /// Calcula y devuelve la lista de cadenas de permisos efectivos atribuibles al usuario.
    /// </summary>
    /// <param name="user">Entidad de usuario sobre la cual se consultarán los permisos.</param>
    /// <returns>Lista de permisos en formato texto.</returns>
    Task<List<string>> GetPermissionsAsync(ApplicationUser user);
}

