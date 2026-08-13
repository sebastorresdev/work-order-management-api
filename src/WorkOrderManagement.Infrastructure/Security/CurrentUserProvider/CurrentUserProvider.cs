using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using WorkOrderManagement.Application.Common.Constants;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Features.Auth.DTOs;

namespace WorkOrderManagement.Infrastructure.Security.CurrentUserProvider;

/// <summary>
/// Implementación de <see cref="ICurrentUserProvider"/> que extrae la información del usuario autenticado desde el <see cref="IHttpContextAccessor"/>.
/// </summary>
/// <param name="_httpContextAccessor">Accesor del contexto HTTP actual.</param>
public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    /// <summary>
    /// Obtiene y mapea los datos del usuario actual (ID, roles y permisos) a partir de los claims del contexto HTTP.
    /// </summary>
    /// <returns>DTO <see cref="CurrentUserResponse"/> con la identidad del usuario.</returns>
    public CurrentUserResponse GetCurrentUser()
    {
        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return new CurrentUserResponse(Guid.Empty, [], []);
        }

        ArgumentNullException.ThrowIfNull(_httpContextAccessor);

        // Extrae el claim del identificador de usuario (NameIdentifier)
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var id))
        {
            return new CurrentUserResponse(Guid.Empty, [], []);
        }

        // Obtiene los roles y permisos del usuario desde sus claims
        var roles = GetClaimValues(ClaimTypes.Role);
        var permissions = GetClaimValues(CustomClaimTypes.Permission);

        return new CurrentUserResponse(id, roles, permissions);
    }

    /// <summary>
    /// Método privado auxiliar para extraer los valores únicos de un tipo de claim específico.
    /// </summary>
    /// <param name="claimType">Tipo de claim a consultar.</param>
    /// <returns>Lista de valores sin duplicados.</returns>
    private List<string> GetClaimValues(string claimType) =>
        [.. _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value).Distinct()];

    /// <summary>
    /// Método privado auxiliar para obtener un único valor de claim.
    /// </summary>
    /// <param name="claimType">Tipo de claim a consultar.</param>
    /// <returns>Valor del claim encontrado.</returns>
    private string GetSingleClaimValue(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}

