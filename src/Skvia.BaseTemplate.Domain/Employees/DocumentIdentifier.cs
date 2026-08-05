using ErrorOr;

namespace Skvia.BaseTemplate.Domain.Employees;

public record DocumentIdentifier
{
    public DocumentType Type { get; init; }
    public string Number { get; init; } = null!;

    private DocumentIdentifier() { }

    private DocumentIdentifier(DocumentType type, string number)
    {
        Type = type;
        Number = number;
    }

    public static ErrorOr<DocumentIdentifier> Create(DocumentType type, string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return Error.Validation("DocumentIdentifier.Empty", "El número de documento no puede estar vacío.");
        }

        var trimmed = number.Trim();
        if (trimmed.Length > EmployeeConstants.DocumentNumberMaxLength)
        {
            return Error.Validation("DocumentIdentifier.TooLong", $"El número de documento excede los {EmployeeConstants.DocumentNumberMaxLength} caracteres.");
        }

        return new DocumentIdentifier(type, trimmed);
    }

    public override string ToString() => $"{Type}: {Number}";
}
