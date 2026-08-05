using System.Reflection;
using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Application.Common.Messaging;
using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Common.Behaviors;

internal static class AuthorizationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ICurrentUserProvider currentUserProvider)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : IErrorOr
    {
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var authorizeAttributes = typeof(TCommand).GetCustomAttributes<HasPermissionAttribute>().ToList();

            if (authorizeAttributes.Count > 0)
            {
                var currentUser = currentUserProvider.GetCurrentUser();

                if (currentUser == null)
                {
                    return (dynamic)Error.Unauthorized("Auth.Unauthorized", "El usuario no está autenticado.");
                }

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

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ICurrentUserProvider currentUserProvider)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IErrorOr
    {
        public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            var authorizeAttributes = typeof(TQuery).GetCustomAttributes<HasPermissionAttribute>().ToList();

            if (authorizeAttributes.Count > 0)
            {
                var currentUser = currentUserProvider.GetCurrentUser();

                if (currentUser == null)
                {
                    return (dynamic)Error.Unauthorized("Auth.Unauthorized", "El usuario no está autenticado.");
                }

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
