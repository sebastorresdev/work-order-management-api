using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Employees.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Employees.Queries.GetEmployeeById;

[HasPermission(Permission.Employee.View)]
public record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery<ErrorOr<EmployeeDetailResponse>>;
