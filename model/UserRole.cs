

namespace Technical_Challenge.model;

public class UserRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
