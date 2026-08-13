using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Employees.DTOs;

namespace WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;

[HasPermission(Permission.Employee.View)]
public record GetEmployeesQuery() : IQuery<ErrorOr<List<EmployeeResponse>>>;
