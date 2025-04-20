using System.ComponentModel.DataAnnotations;

namespace Technical_Challenge.model;

public class AddressBook
{
    [Key]
    public int Id { get; set; }

    public string? FirstName { get; set; } = string.Empty; 
    
    public string? LastName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public DateTime? Birthdate { get; set; } = DateTime.UtcNow;

    public virtual ICollection<user_address_book> User_Address_Books { get; set; } = [];


}