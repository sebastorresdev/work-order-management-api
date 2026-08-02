namespace Skvia.BaseTemplate.Api.Endpoints.Users.Requests;

public record DeleteUsersBatchRequest(List<Guid> UserIds);

