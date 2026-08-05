using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Employees.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Employees.Queries.GetEmployees;

[HasPermission(Permission.Employee.View)]
public record GetEmployeesQuery() : IQuery<ErrorOr<List<EmployeeResponse>>>;
