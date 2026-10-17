using FluentValidation;
using WMS.Application.Features.Orders.Dto;

namespace WMS.Application.Features.Orders.Command.ValidatorsHelper
{
    public class OrderInformationValidator : AbstractValidator<OrderInformation>
    {
        public OrderInformationValidator()
        {
            RuleFor(p => p.ShippingAddress)
                .NotEmpty()
                .MaximumLength(500);
            RuleFor(p => p.PaymentMethod)
                .IsInEnum();
            RuleFor(p => p.Currency)
                .IsInEnum();
        }
    }
}
