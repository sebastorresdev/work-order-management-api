using System.Text.RegularExpressions;

namespace Skvia.BaseTemplate.Domain.Employees;

public readonly partial record struct Email
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email FromDb(string value) => new(value);

    public static ErrorOr<Email> Create(string emailAddress)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            errors.Add(Error.Validation("Email.Empty", "La dirección de correo no puede estar vacía."));
        }

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

    public override string ToString() => Value;
}
