namespace WorkOrderManagement.Application.Common.Messaging;

/// <summary>
/// Contrato para los manejadores que procesan comandos y devuelven un tipo de respuesta específico.
/// </summary>
/// <typeparam name="TCommand">Tipo del comando a procesar.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta esperado.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : IErrorOr
{
    /// <summary>
    /// Procesa de forma asíncrona la ejecución del comando provisto.
    /// </summary>
    /// <param name="command">Instancia del comando con los datos de entrada.</param>
    /// <param name="cancellationToken">Token para monitoreo de cancelación de la operación.</param>
    /// <returns>Tarea asíncrona que contiene la respuesta procesada.</returns>
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

