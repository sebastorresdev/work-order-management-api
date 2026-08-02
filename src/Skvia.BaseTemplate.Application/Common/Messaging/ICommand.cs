namespace Skvia.BaseTemplate.Application.Common.Messaging;

public interface ICommand;

public interface ICommand<TResponse> where TResponse : IErrorOr;

