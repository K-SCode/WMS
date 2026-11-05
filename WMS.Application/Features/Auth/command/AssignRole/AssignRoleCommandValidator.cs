using FluentValidation;

namespace WMS.Application.Features.Auth.command.AssignRole
{
    public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress();
            RuleFor(r => r.Role)
                .NotEmpty();
        }
    }
}
