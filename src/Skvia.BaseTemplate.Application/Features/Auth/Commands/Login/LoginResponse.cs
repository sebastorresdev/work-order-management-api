using System.Security.Claims;

namespace Skvia.BaseTemplate.Application.Features.Auth.Commands.Login;

public record LoginResponse(
    ClaimsPrincipal Principal);

