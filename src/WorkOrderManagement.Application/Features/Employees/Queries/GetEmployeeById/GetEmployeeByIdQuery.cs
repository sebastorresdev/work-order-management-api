using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Employees.DTOs;

namespace WorkOrderManagement.Application.Features.Employees.Queries.GetEmployeeById;

[HasPermission(Permission.Employee.View)]
public record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery<ErrorOr<EmployeeDetailResponse>>;
