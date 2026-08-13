using System.Security.Claims;

using WorkOrderManagement.Application.Common.DTOs;
using WorkOrderManagement.Application.Features.Auth.Commands.Login;
using WorkOrderManagement.Application.Features.Users.Commands.CreateUser;
using WorkOrderManagement.Application.Features.Users.Commands.DeleteUser;
using WorkOrderManagement.Application.Features.Users.Commands.ResetPassword;
using WorkOrderManagement.Application.Features.Users.Commands.SetUserPermissionOverrides;
using WorkOrderManagement.Application.Features.Users.Commands.ToggleUserStatus;
using WorkOrderManagement.Application.Features.Users.Commands.UpdateUser;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Common.Interfaces;

/// <summary>
/// Servicio de aplicación para la administración de cuentas de usuario, autenticación y contraseñas.
/// </summary>
public interface IUserAccountService
{
    /// <summary>
    /// Crea un nuevo usuario en la base de datos de identidad.
    /// </summary>
    /// <param name="command">Comando con los datos requeridos para la creación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador del usuario creado o errores.</returns>
    Task<ErrorOr<Guid>> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza la información de un usuario existente.
    /// </summary>
    /// <param name="command">Comando con los datos a actualizar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Restablece la contraseña de un usuario determinado.
    /// </summary>
    /// <param name="command">Comando con la nueva contraseña.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Elimina o archiva un usuario del sistema.
    /// </summary>
    /// <param name="command">Comando con la solicitud de eliminación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> DeleteUserAsync(DeleteUserCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Establece anulaciones u excepciones explícitas de permisos para un usuario.
    /// </summary>
    /// <param name="command">Comando con las anulaciones de permisos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> SetPermissionOverridesAsync(SetUserPermissionOverridesCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Activa o desactiva el estado de la cuenta de un usuario.
    /// </summary>
    /// <param name="command">Comando con la instrucción de cambio de estado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> ToggleUserStatusAsync(ToggleUserStatusCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Autentica a un usuario comprobando su nombre de usuario y clave.
    /// </summary>
    /// <param name="command">Comando con credenciales de inicio de sesión.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Instancia de <see cref="ClaimsPrincipal"/> si la autenticación es válida o errores.</returns>
    Task<ErrorOr<ClaimsPrincipal>> AuthenticateAsync(LoginCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el detalle completo de un usuario por su identificador.
    /// </summary>
    /// <param name="userId">Identificador único del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO con información de detalle de usuario o errores.</returns>
    Task<ErrorOr<UserDetailResponse>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene la lista completa de usuarios registrados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de DTOs de usuario o errores.</returns>
    Task<ErrorOr<List<UserResponse>>> GetUsersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los permisos del usuario desglosados y agrupados por módulo.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Grupos de permisos asignados al usuario o errores.</returns>
    Task<ErrorOr<List<PermissionGroupResponse>>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken);
}

