using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Domain.Branches;

public class BranchUser : BaseAuditableEntity
{
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}

