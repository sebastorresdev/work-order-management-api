namespace WorkOrderManagement.Domain.Employees;

/// <summary>
/// Objeto de valor (Value Object) que representa la identificación o documento de identidad de un empleado.
/// </summary>
public record DocumentIdentifier
{
    /// <summary>
    /// Tipo de documento de identidad (por ejemplo: DNI, Pasaporte, Cédula de Extranjería).
    /// </summary>
    public DocumentType Type { get; init; }

    /// <summary>
    /// Número o código del documento de identidad.
    /// </summary>
    public string Number { get; init; } = null!;

    /// <summary>
    /// Constructor privado requerido por Entity Framework Core.
    /// </summary>
    private DocumentIdentifier() { }

    /// <summary>
    /// Constructor privado para la inicialización del objeto de valor.
    /// </summary>
    private DocumentIdentifier(DocumentType type, string number)
    {
        Type = type;
        Number = number;
    }

    /// <summary>
    /// Crea e valida una nueva instancia de <see cref="DocumentIdentifier"/>.
    /// </summary>
    /// <param name="type">Tipo de documento de identidad.</param>
    /// <param name="number">Número de documento.</param>
    /// <returns>Una instancia válida de <see cref="DocumentIdentifier"/> o una lista de errores de validación.</returns>
    public static ErrorOr<DocumentIdentifier> Create(DocumentType type, string number)
    {
        // Lista acumuladora de errores de validación
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(number))
        {
            errors.Add(Error.Validation("DocumentIdentifier.Empty", "El número de documento no puede estar vacío."));
        }

        // Variable local con el número de documento limpio de espacios
        var trimmed = number.Trim();
        if (trimmed.Length > EmployeeConstants.DocumentNumberMaxLength)
        {
            errors.Add(Error.Validation("DocumentIdentifier.TooLong", $"El número de documento excede los {EmployeeConstants.DocumentNumberMaxLength} caracteres."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new DocumentIdentifier(type, trimmed);
    }

    /// <summary>
    /// Retorna una representación en texto del tipo y número de documento.
    /// </summary>
    public override string ToString() => $"{Type}: {Number}";
}
