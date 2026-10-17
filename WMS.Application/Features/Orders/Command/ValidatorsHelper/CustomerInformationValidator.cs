using FluentValidation;
using WMS.Application.Features.Orders.Dto;

namespace WMS.Application.Features.Orders.Command.ValidatorsHelper
{
    public class CustomerInformationValidator : AbstractValidator<CustomerInformation>
    {
        public CustomerInformationValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(255)
                .EmailAddress();

            RuleFor(p => p.Phone)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(p => p.Nip)
                .Matches(@"^\d{10}$")
                .When(p => p.Nip is not null);

            RuleFor(p => p.Nip)
                .NotEmpty()
                .When(p => p.CompanyName is not null);

            RuleFor(p => p.CompanyName)
                .NotEmpty()
                .MaximumLength(200)
                .When(p =>  !string.IsNullOrEmpty(p.Nip));
        }
    }
}
