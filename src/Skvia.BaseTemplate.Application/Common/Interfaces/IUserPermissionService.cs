using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

public interface IUserPermissionService
{
    Task<List<string>> GetPermissionsAsync(ApplicationUser user);
}

