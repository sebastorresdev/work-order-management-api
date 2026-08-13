using System.Text.RegularExpressions;

namespace Skvia.BaseTemplate.Domain.Employees;

/// <summary>
/// Objeto de valor (Value Object struct) para la representación y validación de direcciones de correo electrónico.
/// </summary>
public readonly partial record struct Email
{
    /// <summary>
    /// Expresión regular precompilada para la validación del formato de correo electrónico.
    /// </summary>
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    /// <summary>
    /// Valor textual de la dirección de correo electrónico.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor privado para inicializar el valor del correo.
    /// </summary>
    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Reconstruye una instancia de <see cref="Email"/> a partir de un valor persistido en la base de datos sin re-validar.
    /// </summary>
    /// <param name="value">Cadena de correo proveniente de la base de datos.</param>
    /// <returns>Instancia de <see cref="Email"/>.</returns>
    public static Email FromDb(string value) => new(value);

    /// <summary>
    /// Crea e valida una nueva instancia de <see cref="Email"/>.
    /// </summary>
    /// <param name="emailAddress">Cadena con la dirección de correo a validar.</param>
    /// <returns>Una instancia válida de <see cref="Email"/> o una lista de errores de validación.</returns>
    public static ErrorOr<Email> Create(string emailAddress)
    {
        // Lista acumuladora de errores de validación
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            errors.Add(Error.Validation("Email.Empty", "La dirección de correo no puede estar vacía."));
        }

        // Variable local con el correo recortado de espacios
        var trimmed = emailAddress.Trim();

        if (trimmed.Length > EmployeeConstants.EmailMaxLength)
        {
            errors.Add(Error.Validation("Email.TooLong", $"El correo excede la longitud máxima de {EmployeeConstants.EmailMaxLength} caracteres."));
        }

        if (!EmailRegex().IsMatch(trimmed))
        {
            errors.Add(Error.Validation("Email.InvalidFormat", "El formato del correo electrónico es inválido."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Email(trimmed);
    }

    /// <summary>
    /// Devuelve el valor textual del correo electrónico.
    /// </summary>
    public override string ToString() => Value;
}
