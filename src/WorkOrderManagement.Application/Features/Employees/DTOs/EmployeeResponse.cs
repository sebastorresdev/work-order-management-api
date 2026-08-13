using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.DTOs;

/// <summary>
/// DTO de respuesta que contiene la información completa de la ficha de un empleado.
/// </summary>
/// <param name="Id">Identificador único del empleado.</param>
/// <param name="Code">Código interno del empleado.</param>
/// <param name="FirstName">Nombres del empleado.</param>
/// <param name="LastName">Apellidos del empleado.</param>
/// <param name="DocumentType">Tipo de documento de identidad.</param>
/// <param name="DocumentNumber">Número de documento de identidad.</param>
/// <param name="Email">Correo electrónico de contacto opcional.</param>
/// <param name="Phone">Número telefónico de contacto opcional.</param>
/// <param name="Department">Departamento u área asignada.</param>
/// <param name="Position">Cargo o función laboral.</param>
/// <param name="PhotoUrl">URL de la fotografía del empleado.</param>
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

