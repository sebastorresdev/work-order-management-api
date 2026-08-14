using WorkOrderManagement.Application.Features.Employees.DTOs;
using WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository) : IQueryHandler<GetEmployeeByIdQuery, ErrorOr<EmployeeDetailResponse>>
{
    public async Task<ErrorOr<EmployeeDetailResponse>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(query.EmployeeId, cancellationToken);

        if (employee is null)
        {
            return EmployeeErrors.NotFound;
        }

        return employee;
    }
}

