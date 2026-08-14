using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Features.Employees.DTOs;
using WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Infrastructure.Repositories;

public class EmployeeRepository(IApplicationDbContext dbContext) : IEmployeeRepository
{
    public async Task<List<EmployeeResponse>> GetEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .Select(e => new EmployeeResponse(
                Id: e.Id,
                Code: e.Code,
                FirstName: e.FirstName,
                LastName: e.LastName,
                DocumentType: e.DocumentIdentifier.Type,
                DocumentNumber: e.DocumentIdentifier.Number,
                Email: e.Email != null ? e.Email.Value.Value : null,
                Phone: e.Phone != null ? e.Phone.Value.Value : null,
                Department: e.Department,
                Position: e.Position,
                PhotoUrl: e.PhotoUrl))
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeDetailResponse?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .Where(e => e.Id == employeeId)
            .Select(e => new EmployeeDetailResponse(
                e.Id,
                e.Code,
                e.FirstName,
                e.LastName,
                e.DocumentIdentifier.Type,
                e.DocumentIdentifier.Number,
                e.Email != null ? e.Email.Value.Value : null,
                e.Phone != null ? e.Phone.Value.Value : null,
                e.Position,
                e.Department,
                e.HireDate,
                e.PhotoUrl))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await dbContext.Employees
            .AnyAsync(e => e.Code == normalizedCode, cancellationToken);
    }

    public async Task<bool> ExistsByDocumentAsync(DocumentIdentifier documentIdentifier, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AnyAsync(e =>
                e.DocumentIdentifier.Type == documentIdentifier.Type &&
                e.DocumentIdentifier.Number == documentIdentifier.Number &&
                (!excludeId.HasValue || e.Id != excludeId.Value),
                cancellationToken);
    }

    public async Task<Employee?> GetEntityByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
    }

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        dbContext.Employees.Add(employee);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
