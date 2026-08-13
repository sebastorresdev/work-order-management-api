using System.Reflection;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Common.Behaviors;

/// <summary>
/// Proporciona decoradores de autorización para interceptar y verificar permisos en solicitudes de comandos y consultas.
/// </summary>
internal static class AuthorizationDecorator
{
    /// <summary>
    /// Decorador para el manejo de comandos que valida los permisos requeridos (<see cref="HasPermissionAttribute"/>).
    /// </summary>
    /// <typeparam name="TCommand">Tipo de comando procesado.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta del comando.</typeparam>
    /// <param name="innerHandler">Manejador interno de comandos a envolver.</param>
    /// <param name="currentUserProvider">Proveedor del usuario autenticado actual.</param>
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ICurrentUserProvider currentUserProvider)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : IErrorOr
    {
        /// <summary>
        /// Intercepta la ejecución del comando para auditar permisos antes de invocar el manejador interno.
        /// </summary>
        /// <param name="command">Instancia del comando recibido.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta procesada o error de autorización/prohibición (401/403).</returns>
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            // Obtiene los atributos de permiso definidos sobre la clase del comando
            var authorizeAttributes = typeof(TCommand).GetCustomAttributes<HasPermissionAttribute>().ToList();

            if (authorizeAttributes.Count > 0)
            {
                // Consulta los datos del usuario autenticado en la petición
                var currentUser = currentUserProvider.GetCurrentUser();

                if (currentUser == null)
                {
                    return (dynamic)Error.Unauthorized("Auth.Unauthorized", "El usuario no está autenticado.");
                }

                // Evalúa que el usuario posea cada uno de los permisos exigidos por el comando
                foreach (var attr in authorizeAttributes)
                {
                    if (!currentUser.Permissions.Contains(attr.Permission, StringComparer.OrdinalIgnoreCase))
                    {
                        return (dynamic)Error.Forbidden(
                            "Auth.Forbidden",
                            $"No posee el permiso requerido: '{attr.Permission}'.");
                    }
                }
            }

            return await innerHandler.HandleAsync(command, cancellationToken);
        }
    }

    /// <summary>
    /// Decorador para el manejo de consultas que valida los permisos requeridos (<see cref="HasPermissionAttribute"/>).
    /// </summary>
    /// <typeparam name="TQuery">Tipo de consulta procesada.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta de la consulta.</typeparam>
    /// <param name="innerHandler">Manejador interno de consultas a envolver.</param>
    /// <param name="currentUserProvider">Proveedor del usuario autenticado actual.</param>
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ICurrentUserProvider currentUserProvider)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IErrorOr
    {
        /// <summary>
        /// Intercepta la ejecución de la consulta para validar permisos antes de llamar al manejador interno.
        /// </summary>
        /// <param name="query">Instancia de la consulta recibida.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta procesada o error de autorización/prohibición (401/403).</returns>
        public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            // Colección de atributos HasPermission aplicados a la consulta
            var authorizeAttributes = typeof(TQuery).GetCustomAttributes<HasPermissionAttribute>().ToList();

            if (authorizeAttributes.Count > 0)
            {
                // Usuario autenticado actual
                var currentUser = currentUserProvider.GetCurrentUser();

                if (currentUser == null)
                {
                    return (dynamic)Error.Unauthorized("Auth.Unauthorized", "El usuario no está autenticado.");
                }

                // Verificación de presencia de cada permiso
                foreach (var attr in authorizeAttributes)
                {
                    if (!currentUser.Permissions.Contains(attr.Permission, StringComparer.OrdinalIgnoreCase))
                    {
                        return (dynamic)Error.Forbidden(
                            "Auth.Forbidden",
                            $"No posee el permiso requerido: '{attr.Permission}'.");
                    }
                }
            }

            return await innerHandler.HandleAsync(query, cancellationToken);
        }
    }
}
