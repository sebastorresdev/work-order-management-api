namespace Skvia.BaseTemplate.Application.Features.Users.Commands.UploadUserAvatar;

public record UploadUserAvatarCommand(
    Stream FileStream,
    string FileName,
    long FileLength) : ICommand<ErrorOr<FileUploadResponse>>;

