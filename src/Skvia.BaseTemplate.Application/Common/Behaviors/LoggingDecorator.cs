using Microsoft.Extensions.Logging;

using Serilog.Context;

namespace Skvia.BaseTemplate.Application.Common.Behaviors;

/// <summary>
/// Decorador de registro (Logging) para auditar la ejecución de comandos y consultas, registrando tiempos y posibles errores.
/// </summary>
internal static class LoggingDecorator
{
    // =========================================================================
    // 1. DECORADOR PARA TODOS LOS COMANDOS (Commands)
    // =========================================================================
    /// <summary>
    /// Decorador para el manejo de comandos que añade trazabilidad mediante <see cref="ILogger"/>.
    /// </summary>
    /// <typeparam name="TCommand">Tipo del comando.</typeparam>
    /// <typeparam name="TResponse">Tipo de la respuesta del comando.</typeparam>
    /// <param name="innerHandler">Manejador interno del comando.</param>
    /// <param name="logger">Servicio de registro de logs.</param>
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : IErrorOr
    {
        /// <summary>
        /// Procesa el comando registrando el inicio, el éxito o los errores devueltos.
        /// </summary>
        /// <param name="command">Comando en ejecución.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta del comando.</returns>
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            // Nombre del tipo del comando para logs
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            // Invocación del manejador interno
            TResponse result = await innerHandler.HandleAsync(command, cancellationToken);

            if (!result.IsError)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                // Inyección de la lista de errores en el contexto de Serilog
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Completed command {Command} with error(s)", commandName);
                }
            }

            return result;
        }
    }

    // =========================================================================
    // 2. DECORADOR PARA TODAS LAS CONSULTAS (Queries)
    // =========================================================================
    /// <summary>
    /// Decorador para el manejo de consultas que añade trazabilidad mediante <see cref="ILogger"/>.
    /// </summary>
    /// <typeparam name="TQuery">Tipo de la consulta.</typeparam>
    /// <typeparam name="TResponse">Tipo de la respuesta de la consulta.</typeparam>
    /// <param name="innerHandler">Manejador interno de la consulta.</param>
    /// <param name="logger">Servicio de registro de logs.</param>
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IErrorOr
    {
        /// <summary>
        /// Procesa la consulta registrando el inicio, el éxito o los errores producidos.
        /// </summary>
        /// <param name="query">Consulta en ejecución.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta de la consulta.</returns>
        public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            // Nombre del tipo de la consulta para logs
            string queryName = typeof(TQuery).Name;

            logger.LogInformation("Processing query {Query}", queryName);

            // Invocación del manejador interno
            TResponse result = await innerHandler.HandleAsync(query, cancellationToken);

            if (!result.IsError)
            {
                logger.LogInformation("Completed query {Query}", queryName);
            }
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Completed query {Query} with error(s)", queryName);
                }
            }

            return result;
        }
    }
}
