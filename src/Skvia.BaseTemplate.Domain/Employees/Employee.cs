namespace Skvia.BaseTemplate.Domain.Employees;

public class Employee : BaseAuditableEntity, IArchivable
{
    public string Code { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DocumentIdentifier DocumentIdentifier { get; private set; } = null!;
    public Email? Email { get; private set; }
    public Phone? Phone { get; private set; }
    public string? Position { get; private set; }
    public string? Department { get; private set; }
    public DateTimeOffset HireDate { get; private set; }
    public string? PhotoUrl { get; private set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }
    public Guid? ArchivedBy { get; set; }

    private Employee() { }

    public static ErrorOr<Employee> Create(
        string code,
        string firstName,
        string lastName,
        DocumentIdentifier documentIdentifier,
        DateTimeOffset hireDate,
        string? email = null,
        string? phone = null,
        string? position = null,
        string? department = null,
        string? photoUrl = null)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(code) || code.Length > EmployeeConstants.CodeMaxLength)
        {
            errors.Add(Error.Validation("Employee.CodeInvalid", $"El código de empleado es requerido y no debe exceder {EmployeeConstants.CodeMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > EmployeeConstants.FirstNameMaxLength)
        {
            errors.Add(Error.Validation("Employee.FirstNameInvalid", $"El nombre es requerido y no debe exceder {EmployeeConstants.FirstNameMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > EmployeeConstants.LastNameMaxLength)
        {
            errors.Add(Error.Validation("Employee.LastNameInvalid", $"El apellido es requerido y no debe exceder {EmployeeConstants.LastNameMaxLength} caracteres."));
        }

        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Employees.Email.Create(email);
            if (emailResult.IsError) errors.AddRange(emailResult.Errors);
            else emailVo = emailResult.Value;
        }

        Phone? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phone))
        {
            var phoneResult = Employees.Phone.Create(phone);
            if (phoneResult.IsError) errors.AddRange(phoneResult.Errors);
            else phoneVo = phoneResult.Value;
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var employee = new Employee
        {
            Code = code.Trim().ToUpper(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            DocumentIdentifier = documentIdentifier,
            HireDate = hireDate,
            Email = emailVo,
            Phone = phoneVo,
            Position = position?.Trim(),
            Department = department?.Trim(),
            PhotoUrl = photoUrl?.Trim()
        };

        return employee;
    }

    public ErrorOr<Success> Update(
        string code,
        string firstName,
        string lastName,
        DocumentIdentifier documentIdentifier,
        DateTimeOffset hireDate,
        string? email = null,
        string? phone = null,
        string? position = null,
        string? department = null,
        string? photoUrl = null)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(code) || code.Length > EmployeeConstants.CodeMaxLength)
        {
            errors.Add(Error.Validation("Employee.CodeInvalid", $"El código de empleado es requerido y no debe exceder {EmployeeConstants.CodeMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > EmployeeConstants.FirstNameMaxLength)
        {
            errors.Add(Error.Validation("Employee.FirstNameInvalid", $"El nombre es requerido y no debe exceder {EmployeeConstants.FirstNameMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > EmployeeConstants.LastNameMaxLength)
        {
            errors.Add(Error.Validation("Employee.LastNameInvalid", $"El apellido es requerido y no debe exceder {EmployeeConstants.LastNameMaxLength} caracteres."));
        }

        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Employees.Email.Create(email);
            if (emailResult.IsError) errors.AddRange(emailResult.Errors);
            else emailVo = emailResult.Value;
        }

        Phone? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phone))
        {
            var phoneResult = Employees.Phone.Create(phone);
            if (phoneResult.IsError) errors.AddRange(phoneResult.Errors);
            else phoneVo = phoneResult.Value;
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        Code = code.Trim().ToUpper();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DocumentIdentifier = documentIdentifier;
        HireDate = hireDate;
        Email = emailVo;
        Phone = phoneVo;
        Position = position?.Trim();
        Department = department?.Trim();
        PhotoUrl = photoUrl?.Trim();

        return Result.Success;
    }
}
