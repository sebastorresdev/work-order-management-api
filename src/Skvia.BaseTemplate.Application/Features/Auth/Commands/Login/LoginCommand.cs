using System.Security.Claims;

namespace Skvia.BaseTemplate.Application.Features.Auth.Commands.Login;

public record LoginCommand(string UserName, string Password) : ICommand<ErrorOr<ClaimsPrincipal>>;

