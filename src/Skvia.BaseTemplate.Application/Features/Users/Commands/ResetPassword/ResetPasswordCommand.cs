using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.ResetPassword;

[HasPermission(Permission.User.Edit)]
public record ResetPasswordCommand(string UserId, string NewPassword, string ConfirmNewPassword) : ICommand<ErrorOr<Success>>;
