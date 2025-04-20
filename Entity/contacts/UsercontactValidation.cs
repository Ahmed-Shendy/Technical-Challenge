
namespace Technical_Challenge.Entity.contacts;

public class UsercontactValidation : AbstractValidator<Usercontact>
{
    

    public UsercontactValidation()
    {

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(11);


        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

    }
}
