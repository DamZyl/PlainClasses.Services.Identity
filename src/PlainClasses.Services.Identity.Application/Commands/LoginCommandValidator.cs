using FluentValidation;

namespace PlainClasses.Services.Identity.Application.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.PersonalNumber)
                .NotEmpty()
                .WithMessage("PersonalNumber is empty.");
            RuleFor(x => x.PersonalNumber)
                .Length(11)
                .WithMessage("PersonalNumber should have 11 digits.");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is empty.");
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Password should have greater than 8 letters.");
        }
    }
}