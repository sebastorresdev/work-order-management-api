using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WorkOrderManagement.Application.Common.Constants;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Features.Auth.DTOs;

namespace WorkOrderManagement.Infrastructure.Security.CurrentUserProvider;

/// <summary>
/// Implementación de <see cref="ICurrentUserProvider"/> que extrae la información del usuario autenticado desde el <see cref="IHttpContextAccessor"/>.
/// </summary>
/// <param name="_httpContextAccessor">Accesor del contexto HTTP actual.</param>
/// <param name="_serviceProvider">
/// Proveedor de servicios usado para resolver <see cref="IApplicationDbContext"/> de forma perezosa.
/// Se evita la inyección directa por constructor porque los interceptores de <c>ApplicationDbContext</c>
/// (AuditableEntityInterceptor/AuditTrailInterceptor) dependen de <see cref="ICurrentUserProvider"/>;
/// si este dependiera directamente de <see cref="IApplicationDbContext"/>, se produciría un ciclo de
/// resolución en el mismo scope (ApplicationDbContext -> interceptors -> CurrentUserProvider -> ApplicationDbContext)
/// que cuelga el contenedor de DI de forma silenciosa al construir el DbContext.
/// </param>
public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor, IServiceProvider _serviceProvider) : ICurrentUserProvider
{
    /// <summary>
    /// Obtiene y mapea los datos del usuario actual (ID, roles, permisos y sedes asignadas) a partir del contexto HTTP y la base de datos.
    /// </summary>
    /// <returns>DTO <see cref="CurrentUserResponse"/> con la identidad del usuario.</returns>
    public CurrentUserResponse GetCurrentUser()
    {
        if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return new CurrentUserResponse(Guid.Empty, [], [], []);
        }

        ArgumentNullException.ThrowIfNull(_httpContextAccessor);

        // Extrae el claim del identificador de usuario (NameIdentifier)
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var id))
        {
            return new CurrentUserResponse(Guid.Empty, [], [], []);
        }

        // Obtiene los roles y permisos del usuario desde sus claims
        var roles = GetClaimValues(ClaimTypes.Role);
        var permissions = GetClaimValues(CustomClaimTypes.Permission);

        var dbContext = _serviceProvider.GetRequiredService<IApplicationDbContext>();
        var user = dbContext.ApplicationUsers
            .AsNoTracking()
            .Include(u => u.BranchUsers)
            .FirstOrDefault(u => u.Id == id);

        var branchIds = new List<Guid>();
        if (user is not null)
        {
            if (user.BranchId.HasValue)
            {
                branchIds.Add(user.BranchId.Value);
            }

            foreach (var branchUser in user.BranchUsers.Where(bu => bu.BranchId != Guid.Empty))
            {
                branchIds.Add(branchUser.BranchId);
            }
        }

        return new CurrentUserResponse(id, roles, permissions, branchIds.Distinct().ToList());
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

