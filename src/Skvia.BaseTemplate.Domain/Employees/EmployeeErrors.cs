namespace Skvia.BaseTemplate.Domain.Employees;

/// <summary>
/// Contiene definiciones estáticas de errores de dominio específicos de empleados.
/// </summary>
public static class EmployeeErrors
{
    /// <summary>
    /// Genera un error indicando que el código de empleado ya está registrado.
    /// </summary>
    /// <param name="code">Código de empleado en conflicto.</param>
    /// <returns>Error de conflicto con descripción personalizada.</returns>
    public static Error CodeExists(string code) => Error.Conflict(
        code: "Employee.CodeExists",
        description: $"El código de empleado '{code}' ya se encuentra registrado.");

    /// <summary>
    /// Genera un error indicando que el documento de identidad ya está registrado.
    /// </summary>
    /// <param name="documentNumber">Número de documento en conflicto.</param>
    /// <returns>Error de conflicto con descripción personalizada.</returns>
    public static Error DocumentExists(string documentNumber) => Error.Conflict(
        code: "Employee.DocumentExists",
        description: $"El documento de identidad '{documentNumber}' ya está asignado a otro empleado.");

    /// <summary>
    /// Error predefinido cuando el empleado no existe en la base de datos.
    /// </summary>
    public static Error NotFound => Error.NotFound(
        code: "Employee.NotFound",
        description: "El empleado no se encuentra registrado.");
}

