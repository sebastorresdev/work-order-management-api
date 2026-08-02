using Skvia.BaseTemplate.Application.Features.Employees.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery() : IQuery<ErrorOr<List<EmployeeResponse>>>;

