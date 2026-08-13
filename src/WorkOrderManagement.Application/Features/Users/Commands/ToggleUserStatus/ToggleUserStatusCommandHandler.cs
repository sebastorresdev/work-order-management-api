using WorkOrderManagement.Application.Common.Interfaces;

namespace WorkOrderManagement.Application.Features.Users.Commands.ToggleUserStatus;

public class ToggleUserStatusCommandHandler(IUserAccountService userAccountService)
    : ICommandHandler<ToggleUserStatusCommand, ErrorOr<Success>>
{
    public Task<ErrorOr<Success>> HandleAsync(ToggleUserStatusCommand command, CancellationToken cancellationToken)
        => userAccountService.ToggleUserStatusAsync(command, cancellationToken);
}

