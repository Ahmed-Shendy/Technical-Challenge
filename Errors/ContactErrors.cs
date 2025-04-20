using Technical_Challenge.Abstractions;

namespace Technical_Challenge.Errors;

public static class ContactErrors
{
    public static readonly Error ContactNotFound =
        new("Contact.NotFound", "No Contact was found ", StatusCodes.Status404NotFound);

    

    public static readonly Error EmailUnque =
    new("Contact.EmailUnque", "Email must unique", StatusCodes.Status400BadRequest);

    public static readonly Error PhoneUnque =
    new("Contact.PhoneUnque", "Phone must unique", StatusCodes.Status400BadRequest);

    public static readonly Error NoContact =
    new("Contact.NoContact", "the user not have Contact", StatusCodes.Status400BadRequest);



}

