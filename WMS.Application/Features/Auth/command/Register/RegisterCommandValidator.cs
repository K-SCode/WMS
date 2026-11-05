using FluentValidation;

namespace WMS.Application.Features.Auth.command.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress();
            RuleFor(r => r.Password)
                .NotEmpty();
            RuleFor(r => r.RepeatPassword)
                .NotEmpty();
            RuleFor(r => r.Phone)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(r => r.FirstName)
                .NotEmpty();
            RuleFor(r => r.LastName)
                .NotEmpty();
        }
    }
}
