using ErrorOr;

namespace WorkOrderManagement.Domain.Branches;

/// <summary>
/// Proporciona definiciones centralizadas de errores de dominio relacionados con sedes.
/// </summary>
public static class BranchErrors
{
    /// <summary>
    /// Genera un error de conflicto indicando que el código de la sede ya existe.
    /// </summary>
    /// <param name="code">Código de la sede que se encuentra duplicado.</param>
    /// <returns>Objeto de error de tipo Conflict con detalle sobre la sede duplicada.</returns>
    public static Error DuplicateBranch(string code) =>
        Error.Conflict(
            code: "Branch.DuplicateBranch",
            description: $"El Codigo de la sede '{code}' ya está en uso.");

    /// <summary>
    /// Error estándar cuando la sede solicitada no es encontrada en el sistema.
    /// </summary>
    public static Error NotFound =>
        Error.NotFound(
            code: "Branch.NotFound",
            description: $"Sede no encontrada.");
}

