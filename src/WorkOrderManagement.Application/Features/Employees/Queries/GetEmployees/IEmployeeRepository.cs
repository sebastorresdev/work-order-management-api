using WorkOrderManagement.Application.Features.Employees.DTOs;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;

public interface IEmployeeRepository
{
    Task<List<EmployeeResponse>> GetEmployeesAsync(CancellationToken cancellationToken = default);
    Task<EmployeeDetailResponse?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDocumentAsync(DocumentIdentifier documentIdentifier, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<Employee?> GetEntityByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
