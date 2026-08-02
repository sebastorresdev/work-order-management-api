using Skvia.BaseTemplate.Application.Features.Employees.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery<ErrorOr<EmployeeDetailResponse>>;

