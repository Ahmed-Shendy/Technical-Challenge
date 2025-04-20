using System.ComponentModel.DataAnnotations.Schema;

namespace Technical_Challenge.model;

public class user_address_book
{

    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AddressBook))]
    public int AddressBookId { get; set; } 

    public User User { get; set; } 
    public AddressBook AddressBook { get; set; } 
}
