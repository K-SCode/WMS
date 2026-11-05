using FluentValidation;

namespace WMS.Application.Features.Auth.command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() 
        {
            RuleFor(l => l.Email)
                .EmailAddress();
            RuleFor(l => l.Password)
                .NotEmpty();
        }
    }
}
