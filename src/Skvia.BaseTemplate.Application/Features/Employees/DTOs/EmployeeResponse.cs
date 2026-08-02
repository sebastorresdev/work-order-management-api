using Skvia.BaseTemplate.Domain.Employees;

namespace Skvia.BaseTemplate.Application.Features.Employees.DTOs;

public record EmployeeResponse(
    Guid Id,
    string Code,
    string FirstName,
    string LastName,
    DocumentType DocumentType,
    string DocumentNumber,
    string? Email,
    string? Phone,
    string? Department,
    string? Position,
    string? PhotoUrl);

