using Skvia.BaseTemplate.Domain.Employees;

namespace Skvia.BaseTemplate.Application.Features.Employees.DTOs;

public record EmployeeDetailResponse(
    Guid Id,
    string Code,
    string FirstName,
    string LastName,
    DocumentType DocumentType,
    string DocumentNumber,
    string? Email,
    string? Phone,
    string? Position,
    string? Department,
    DateTimeOffset HireDate,
    string? PhotoUrl);

