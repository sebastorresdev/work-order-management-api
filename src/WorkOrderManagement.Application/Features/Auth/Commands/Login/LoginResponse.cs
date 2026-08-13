using System.Security.Claims;

namespace WorkOrderManagement.Application.Features.Auth.Commands.Login;

public record LoginResponse(
    ClaimsPrincipal Principal);

