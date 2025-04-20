using System.ComponentModel.DataAnnotations;

namespace Technical_Challenge.Entity.contacts;

public class Usercontact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public DateTime? Birthdate { get; set; }
}
