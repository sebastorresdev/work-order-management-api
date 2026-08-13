namespace WorkOrderManagement.Application.Common.Messaging;

/// <summary>
/// Interfaz marcadora para comandos que ejecutan acciones o mutaciones de estado sin retornar una respuesta específica.
/// </summary>
public interface ICommand;

/// <summary>
/// Interfaz marcadora para comandos que ejecutan mutaciones y retornan un resultado envuelto en <see cref="IErrorOr"/>.
/// </summary>
/// <typeparam name="TResponse">Tipo de respuesta retornado por el comando.</typeparam>
public interface ICommand<TResponse> where TResponse : IErrorOr;

