using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.UploadUserAvatar;

[HasPermission(Permission.User.Edit)]
public record UploadUserAvatarCommand(
    Stream FileStream,
    string FileName,
    long FileLength) : ICommand<ErrorOr<FileUploadResponse>>;
