namespace Skvia.BaseTemplate.Application.Common.Messaging;

/// <summary>
/// Interfaz marcadora para consultas (Queries) de lectura que devuelven un tipo de respuesta específico envuelto en <see cref="IErrorOr"/>.
/// </summary>
/// <typeparam name="TResponse">Tipo de datos retornado por la consulta.</typeparam>
public interface IQuery<out TResponse> where TResponse : IErrorOr;

