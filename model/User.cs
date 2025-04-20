using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Technical_Challenge.model;

public class User : IdentityUser
{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<user_address_book> User_Address_Books { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];


}
