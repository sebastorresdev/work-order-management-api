namespace Skvia.BaseTemplate.Domain.Employees;

/// <summary>
/// Tipos de documento de identidad admitidos por el sistema.
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// Documento Nacional de Identidad.
    /// </summary>
    Dni = 0,

    /// <summary>
    /// Cédula o Carnet de Extranjería.
    /// </summary>
    Ce = 1,

    /// <summary>
    /// Pasaporte.
    /// </summary>
    Passport = 2,
}

