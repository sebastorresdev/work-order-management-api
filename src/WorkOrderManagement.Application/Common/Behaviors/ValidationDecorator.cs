using FluentValidation.Results;

namespace WorkOrderManagement.Application.Common.Behaviors;

/// <summary>
/// Decorador para la validación automática de comandos mediante contratos de FluentValidation.
/// </summary>
internal static class ValidationDecorator
{
    /// <summary>
    /// Decorador para el manejo de comandos que ejecuta la colección de validadores previa procesación del comando.
    /// </summary>
    /// <typeparam name="TCommand">Tipo del comando.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta devuelto.</typeparam>
    /// <param name="innerHandler">Manejador interno de comandos.</param>
    /// <param name="validators">Lista de validadores de FluentValidation inyectados.</param>
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : IErrorOr
    {
        /// <summary>
        /// Valida el comando antes de llamar al manejador interno. Si existen fallas de validación, retorna errores.
        /// </summary>
        /// <param name="command">Comando recibido.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Resultado del comando o lista de errores de validación.</returns>
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            // Ejecuta asincrónicamente la validación del comando
            var failures = await ValidateAsync(command, validators);

            if (failures.Count == 0)
            {
                return await innerHandler.HandleAsync(command, cancellationToken);
            }

            // Mapea los fallos de FluentValidation a objetos Error de ErrorOr
            var errors = failures.ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

            return (dynamic)errors;
        }
    }

    /// <summary>
    /// Método privado auxiliar para invocar secuencialmente/en paralelo la lista de validadores.
    /// </summary>
    /// <typeparam name="TCommand">Tipo de comando.</typeparam>
    /// <param name="command">Instancia del comando a validar.</param>
    /// <param name="validators">Colección de validadores.</param>
    /// <returns>Lista de errores de validación encontrados.</returns>
    private static async Task<List<ValidationFailure>> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any()) return [];

        // Contexto de validación para FluentValidation
        var context = new ValidationContext<TCommand>(command);

        // Ejecución en paralelo de todos los validadores inyectados
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context)));

        return [.. validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)];
    }
}
