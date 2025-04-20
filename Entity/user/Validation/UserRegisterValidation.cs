using FluentValidation;

namespace Technical_Challenge.Entity.user.Validation;

public class UserRegisterValidation : AbstractValidator<UserRegister>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[a-z])(?=(.*[A-Z]))";

    public UserRegisterValidation()
    {

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 100)
            .Matches(Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase , Digites and Uppercase");

    }
}
