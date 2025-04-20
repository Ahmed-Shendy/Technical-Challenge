using System.ComponentModel.DataAnnotations;

namespace Technical_Challenge.Entity.user;

public class userLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
