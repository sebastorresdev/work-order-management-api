using System.Security.Claims;

using Skvia.BaseTemplate.Application.Common.DTOs;
using Skvia.BaseTemplate.Application.Features.Auth.Commands.Login;
using Skvia.BaseTemplate.Application.Features.Users.Commands.CreateUser;
using Skvia.BaseTemplate.Application.Features.Users.Commands.DeleteUser;
using Skvia.BaseTemplate.Application.Features.Users.Commands.ResetPassword;
using Skvia.BaseTemplate.Application.Features.Users.Commands.SetUserPermissionOverrides;
using Skvia.BaseTemplate.Application.Features.Users.Commands.ToggleUserStatus;
using Skvia.BaseTemplate.Application.Features.Users.Commands.UpdateUser;
using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

public interface IUserAccountService
{
    Task<ErrorOr<Guid>> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<Success>> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<Success>> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<Success>> DeleteUserAsync(DeleteUserCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<Success>> SetPermissionOverridesAsync(SetUserPermissionOverridesCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<Success>> ToggleUserStatusAsync(ToggleUserStatusCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<ClaimsPrincipal>> AuthenticateAsync(LoginCommand command, CancellationToken cancellationToken);
    Task<ErrorOr<UserDetailResponse>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<List<UserResponse>>> GetUsersAsync(CancellationToken cancellationToken);
    Task<ErrorOr<List<PermissionGroupResponse>>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken);
}

