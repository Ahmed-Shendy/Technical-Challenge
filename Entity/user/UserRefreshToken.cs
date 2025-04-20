using System.ComponentModel.DataAnnotations;

namespace Technical_Challenge.Entity.user;


public class UserRefreshToken
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefrehToken { get; set; }

}
