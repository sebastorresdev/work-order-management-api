namespace Skvia.BaseTemplate.Application.Common.Messaging;

public interface IQuery<out TResponse> where TResponse : IErrorOr;

