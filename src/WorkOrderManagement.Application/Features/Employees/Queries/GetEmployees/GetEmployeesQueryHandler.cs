using WorkOrderManagement.Application.Features.Employees.DTOs;

namespace WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IQueryHandler<GetEmployeesQuery, ErrorOr<List<EmployeeResponse>>>
{
    public async Task<ErrorOr<List<EmployeeResponse>>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        return await employeeRepository.GetEmployeesAsync(cancellationToken);
    }
}

