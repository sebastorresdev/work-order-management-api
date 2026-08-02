namespace Skvia.BaseTemplate.Api.Endpoints.Users.Requests;

public record ResetPasswordRequest(string UserId, string NewPassword, string ConfirmNewPassword);

