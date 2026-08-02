using Microsoft.AspNetCore.Identity;

namespace Skvia.BaseTemplate.Domain.Identity;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
}

