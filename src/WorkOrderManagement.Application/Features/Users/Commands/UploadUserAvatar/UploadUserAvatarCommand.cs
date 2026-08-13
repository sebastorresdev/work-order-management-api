using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.UploadUserAvatar;

[HasPermission(Permission.User.Edit)]
public record UploadUserAvatarCommand(
    Stream FileStream,
    string FileName,
    long FileLength) : ICommand<ErrorOr<FileUploadResponse>>;
