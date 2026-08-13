namespace Skvia.BaseTemplate.Domain.Identity;

/// <summary>
/// Proporciona las definiciones centralizadas de errores de dominio y autenticación relacionados con usuarios.
/// </summary>
public static class UserErrors
{
    /// <summary>
    /// Indica que las credenciales proporcionadas son inválidas.
    /// </summary>
    /// <returns>Un objeto <see cref="Error"/> configurado como un <c>Unauthorized</c> (HTTP 401).</returns>
    public static Error InvalidCredentials =>
       Error.Unauthorized(
           code: "Login.InvalidCredentials",
           description: "El nombre de usuario o la contraseña son incorrectos.");

    /// <summary>
    /// Indica que el nombre de usuario proporcionado ya está en uso.
    /// </summary>
    /// <param name="userName">Nombre de usuario duplicado.</param>
    /// <returns>Un objeto <see cref="Error"/> configurado como un <c>Conflict</c> (HTTP 409).</returns>
    public static Error DuplicateUser(string userName) =>
        Error.Conflict(
            code: "User.DuplicateUser",
            description: $"El nombre de usuario '{userName}' ya está en uso.");

    /// <summary>
    /// Indica que no se encontró un usuario con el ID proporcionado.
    /// </summary>
    /// <returns>Un objeto <see cref="Error"/> configurado como un <c>NotFound</c> (HTTP 404).</returns>
    public static Error UserNotFound =>
        Error.NotFound(
            code: "User.UserNotFound",
            description: $"Usuario no encontrado.");

    /// <summary>
    /// Indica que la cuenta del usuario está bloqueada temporalmente debido a demasiados intentos fallidos de inicio de sesión.
    /// </summary>
    /// <param name="lockoutEnd">Fecha y hora de finalización del bloqueo temporal.</param>
    /// <returns>Objeto de error indicando el tiempo restante de bloqueo.</returns>
    public static Error AccountLocked(DateTimeOffset? lockoutEnd) => Error.Validation(
        code: "User.AccountLocked",
        description: lockoutEnd.HasValue
            ? $"Tu cuenta ha sido bloqueada temporalmente por demasiados intentos fallidos. Intenta de nuevo después de las {lockoutEnd.Value.ToLocalTime():hh:mm tt}."
            : "Tu cuenta se encuentra bloqueada temporalmente. Intenta más tarde.");

    /// <summary>
    /// Error que se produce cuando el contexto del usuario actual no es válido, 
    /// indicando que el token JWT no está presente, ha expirado o está mal formado.
    /// </summary>
    public static Error Unauthenticated => Error.Unauthorized(
        code: "User.Unauthenticated",
        description: "El usuario no se encuentra autenticado o la sesión ha expirado.");

    /// <summary>
    /// Error inesperado o de excepción general ocurrido en operaciones de usuario.
    /// </summary>
    /// <param name="message">Detalle técnico del error inesperado.</param>
    /// <returns>Objeto de error de tipo Unexpected.</returns>
    public static Error UnexpectedError(string message) =>
        Error.Unexpected(code: "UserException", description: message);
}

