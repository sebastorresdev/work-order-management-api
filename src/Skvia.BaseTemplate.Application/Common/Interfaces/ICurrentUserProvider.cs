using Skvia.BaseTemplate.Application.Features.Auth.DTOs;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CurrentUserResponse GetCurrentUser();
}

