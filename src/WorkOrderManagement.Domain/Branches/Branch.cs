namespace WorkOrderManagement.Domain.Branches;

/// <summary>
/// Entidad de dominio que representa una sede o sucursal de la organización.
/// </summary>
public class Branch : BaseAuditableEntity, IArchivable
{
    /// <summary>
    /// Código único identificador de la sede.
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Nombre descriptivo o comercial de la sede.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Dirección física opcional de la sede.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Indica si la sede ha sido archivada o desactivada lógicamente.
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// Fecha y hora en UTC cuando la sede fue archivada.
    /// </summary>
    public DateTimeOffset? ArchivedAt { get; set; }

    /// <summary>
    /// Identificador del usuario que realizó la acción de archivar la sede.
    /// </summary>
    public Guid? ArchivedBy { get; set; }

    /// <summary>
    /// Lista interna de asociaciones entre esta sede y los usuarios asignados.
    /// </summary>
    private readonly List<BranchUser> _branchUsers = [];

    /// <summary>
    /// Colección de solo lectura de los usuarios vinculados a esta sede.
    /// </summary>
    public IReadOnlyCollection<BranchUser> BranchUsers => _branchUsers.AsReadOnly();

    /// <summary>
    /// Constructor privado requerido para la instanciación mediante Entity Framework Core.
    /// </summary>
    private Branch() { } // EF Core

    /// <summary>
    /// Crea e inicializa una nueva instancia de la entidad sede con validación de datos.
    /// </summary>
    /// <param name="code">Código identificador único de la sede.</param>
    /// <param name="name">Nombre descriptivo de la sede.</param>
    /// <param name="address">Dirección física opcional de la sede.</param>
    /// <returns>Una instancia de <see cref="Branch"/> si es exitoso o una lista de errores de validación.</returns>
    public static ErrorOr<Branch> Create(string code, string name, string? address = null)
    {
        // Acumula errores de validación encontrados durante la verificación de parámetros
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(code) || code.Length > BranchConstants.CodeMaxLength)
        {
            errors.Add(Error.Validation("Branch.CodeInvalid", $"El código de la sede es requerido y no debe exceder {BranchConstants.CodeMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(name) || name.Length > BranchConstants.NameMaxLength)
        {
            errors.Add(Error.Validation("Branch.NameInvalid", $"El nombre de la sede es requerido y no debe exceder {BranchConstants.NameMaxLength} caracteres."));
        }

        if (address != null && address.Length > BranchConstants.AddressMaxLength)
        {
            errors.Add(Error.Validation("Branch.AddressInvalid", $"La dirección no debe exceder {BranchConstants.AddressMaxLength} caracteres."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        // Instancia de la nueva sede sanitizando los datos de entrada
        var branch = new Branch
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            Address = address?.Trim()
        };

        return branch;
    }

    /// <summary>
    /// Actualiza los datos principales de la sede existente con validación previa.
    /// </summary>
    /// <param name="code">Nuevo código para la sede.</param>
    /// <param name="name">Nuevo nombre para la sede.</param>
    /// <param name="address">Nueva dirección opcional para la sede.</param>
    /// <returns>Resultado de éxito o lista de errores de validación.</returns>
    public ErrorOr<Success> Update(string code, string name, string? address = null)
    {
        // Acumula errores de validación durante la actualización
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(code) || code.Length > BranchConstants.CodeMaxLength)
        {
            errors.Add(Error.Validation("Branch.CodeInvalid", $"El código de la sede es requerido y no debe exceder {BranchConstants.CodeMaxLength} caracteres."));
        }

        if (string.IsNullOrWhiteSpace(name) || name.Length > BranchConstants.NameMaxLength)
        {
            errors.Add(Error.Validation("Branch.NameInvalid", $"El nombre de la sede es requerido y no debe exceder {BranchConstants.NameMaxLength} caracteres."));
        }

        if (address != null && address.Length > BranchConstants.AddressMaxLength)
        {
            errors.Add(Error.Validation("Branch.AddressInvalid", $"La dirección no debe exceder {BranchConstants.AddressMaxLength} caracteres."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Address = address?.Trim();

        return Result.Success;
    }
}
