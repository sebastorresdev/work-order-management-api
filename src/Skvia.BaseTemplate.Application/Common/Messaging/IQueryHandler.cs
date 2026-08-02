namespace Skvia.BaseTemplate.Application.Common.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : IErrorOr
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

