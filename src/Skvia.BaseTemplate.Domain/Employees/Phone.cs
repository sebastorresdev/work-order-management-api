using System.Text.RegularExpressions;

namespace Skvia.BaseTemplate.Domain.Employees;

public readonly partial record struct Phone
{
    [GeneratedRegex(@"^\+?[\d\s-]{7,20}$")]
    private static partial Regex PhoneRegex();

    public string Value { get; }

    private Phone(string value)
    {
        Value = value;
    }

    public static Phone FromDb(string value) => new(value);

    public static ErrorOr<Phone> Create(string phoneNumber)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            errors.Add(Error.Validation("Phone.Empty", "El número de teléfono no puede estar vacío."));
        }

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

    public override string ToString() => Value;
}
