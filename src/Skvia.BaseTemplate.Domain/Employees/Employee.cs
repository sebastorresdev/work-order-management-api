namespace Skvia.BaseTemplate.Domain.Employees;

/// <summary>
/// Entidad de dominio que representa a un empleado de la organización.
/// </summary>
public class Employee : BaseAuditableEntity, IArchivable
{
    /// <summary>
    /// Código interno único asignado al empleado.
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Nombres del empleado.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Apellidos del empleado.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Identificación de documento de identidad (tipo y número).
    /// </summary>
    public DocumentIdentifier DocumentIdentifier { get; private set; } = null!;

    /// <summary>
    /// Correo electrónico institucional o personal opcional del empleado.
    /// </summary>
    public Email? Email { get; private set; }

    /// <summary>
    /// Número de teléfono de contacto opcional del empleado.
    /// </summary>
    public Phone? Phone { get; private set; }

    /// <summary>
    /// Cargo o posición laboral que desempeña el empleado.
    /// </summary>
    public string? Position { get; private set; }

    /// <summary>
    /// Departamento o área organizacional a la que pertenece el empleado.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// Fecha de contratación o ingreso a la empresa.
    /// </summary>
    public DateTimeOffset HireDate { get; private set; }

    /// <summary>
    /// URL de la imagen de perfil o foto de la ficha del empleado.
    /// </summary>
    public string? PhotoUrl { get; private set; }

    /// <summary>
    /// Estado de archivado o borrado lógico del empleado.
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// Marca de tiempo en UTC indicando cuándo fue archivado el empleado.
    /// </summary>
    public DateTimeOffset? ArchivedAt { get; set; }

    /// <summary>
    /// Identificador del usuario que realizó la acción de archivar al empleado.
    /// </summary>
    public Guid? ArchivedBy { get; set; }

    /// <summary>
    /// Constructor privado requerido para Entity Framework Core.
    /// </summary>
    private Employee() { }

    /// <summary>
    /// Método de fábrica para crear e inicializar un nuevo empleado previa validación de datos.
    /// </summary>
    /// <param name="code">Código único de empleado.</param>
    /// <param name="firstName">Nombres del empleado.</param>
    /// <param name="lastName">Apellidos del empleado.</param>
    /// <param name="documentIdentifier">Objeto de valor con el documento de identidad.</param>
    /// <param name="hireDate">Fecha de contratación.</param>
    /// <param name="email">Cadena opcional con el correo electrónico.</param>
    /// <param name="phone">Cadena opcional con el teléfono.</param>
    /// <param name="position">Cargo u ocupación del empleado.</param>
    /// <param name="department">Departamento de asignación.</param>
    /// <param name="photoUrl">Ruta o URL de la fotografía.</param>
    /// <returns>Instancia de <see cref="Employee"/> o lista de errores de validación.</returns>
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
        // Colección de errores detectados durante las validaciones de campos
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

        // Creación y validación del Value Object de Email si se suministró
        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Employees.Email.Create(email);
            if (emailResult.IsError) errors.AddRange(emailResult.Errors);
            else emailVo = emailResult.Value;
        }

        // Creación y validación del Value Object de Phone si se suministró
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

        // Instancia del objeto empleado sanitizando los textos
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

    /// <summary>
    /// Actualiza la información personal y laboral del empleado existente.
    /// </summary>
    /// <param name="code">Nuevo código de empleado.</param>
    /// <param name="firstName">Nombres del empleado.</param>
    /// <param name="lastName">Apellidos del empleado.</param>
    /// <param name="documentIdentifier">Objeto de valor con la información del documento de identidad.</param>
    /// <param name="hireDate">Fecha de contratación.</param>
    /// <param name="email">Correo electrónico actualizado opcional.</param>
    /// <param name="phone">Teléfono actualizado opcional.</param>
    /// <param name="position">Nuevo cargo u ocupación.</param>
    /// <param name="department">Nuevo departamento.</param>
    /// <param name="photoUrl">Nueva URL de fotografía de perfil.</param>
    /// <returns>Resultado de éxito o lista de errores de validación.</returns>
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
        // Colección de errores para la operación de actualización
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

        // Validación del objeto Email
        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Employees.Email.Create(email);
            if (emailResult.IsError) errors.AddRange(emailResult.Errors);
            else emailVo = emailResult.Value;
        }

        // Validación del objeto Phone
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
