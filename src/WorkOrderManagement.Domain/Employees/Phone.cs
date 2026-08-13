using System.Text.RegularExpressions;

namespace WorkOrderManagement.Domain.Employees;

/// <summary>
/// Objeto de valor (Value Object struct) para la representación y validación de números telefónicos.
/// </summary>
public readonly partial record struct Phone
{
    /// <summary>
    /// Expresión regular precompilada para la validación del formato telefónico.
    /// </summary>
    [GeneratedRegex(@"^\+?[\d\s-]{7,20}$")]
    private static partial Regex PhoneRegex();

    /// <summary>
    /// Valor textual del número telefónico.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor privado para inicializar el valor del teléfono.
    /// </summary>
    private Phone(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Reconstruye una instancia de <see cref="Phone"/> desde la base de datos sin volver a validar.
    /// </summary>
    /// <param name="value">Número de teléfono proveniente de la base de datos.</param>
    /// <returns>Instancia de <see cref="Phone"/>.</returns>
    public static Phone FromDb(string value) => new(value);

    /// <summary>
    /// Crea e valida una nueva instancia de <see cref="Phone"/>.
    /// </summary>
    /// <param name="phoneNumber">Número telefónico a validar.</param>
    /// <returns>Una instancia válida de <see cref="Phone"/> o una lista de errores de validación.</returns>
    public static ErrorOr<Phone> Create(string phoneNumber)
    {
        // Lista acumuladora de errores de validación
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            errors.Add(Error.Validation("Phone.Empty", "El número de teléfono no puede estar vacío."));
        }

        // Variable local con el número recortado de espacios
        var trimmed = phoneNumber.Trim();

        if (trimmed.Length > EmployeeConstants.PhoneMaxLength)
        {
            errors.Add(Error.Validation("Phone.TooLong", $"El número de teléfono excede los {EmployeeConstants.PhoneMaxLength} caracteres."));
        }

        if (!PhoneRegex().IsMatch(trimmed))
        {
            errors.Add(Error.Validation("Phone.InvalidFormat", "El formato del teléfono es inválido."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Phone(trimmed);
    }

    /// <summary>
    /// Devuelve la cadena que representa el número telefónico.
    /// </summary>
    public override string ToString() => Value;
}
