namespace Skvia.BaseTemplate.Domain.Branches;

public class Branch : BaseAuditableEntity, IArchivable
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Address { get; private set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }
    public Guid? ArchivedBy { get; set; }

    private readonly List<BranchUser> _branchUsers = [];
    public IReadOnlyCollection<BranchUser> BranchUsers => _branchUsers.AsReadOnly();

    private Branch() { } // EF Core

    public static ErrorOr<Branch> Create(string code, string name, string? address = null)
    {
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

        var branch = new Branch
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            Address = address?.Trim()
        };

        return branch;
    }

    public ErrorOr<Success> Update(string code, string name, string? address = null)
    {
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
