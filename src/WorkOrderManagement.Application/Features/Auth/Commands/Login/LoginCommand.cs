using System.Security.Claims;

namespace WorkOrderManagement.Application.Features.Auth.Commands.Login;

public record LoginCommand(string UserName, string Password) : ICommand<ErrorOr<ClaimsPrincipal>>;

