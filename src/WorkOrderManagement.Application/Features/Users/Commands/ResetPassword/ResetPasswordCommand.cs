using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.ResetPassword;

[HasPermission(Permission.User.Edit)]
public record ResetPasswordCommand(string UserId, string NewPassword, string ConfirmNewPassword) : ICommand<ErrorOr<Success>>;
