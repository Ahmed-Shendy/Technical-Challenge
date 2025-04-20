

namespace Technical_Challenge.Entity.user;

public class UserRespones
{

    public string FirstName { get; set; } = null!;
    public string Id { get; set; }
    public string Email { get; set; } = null!;
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiretion { get; set; }

}
