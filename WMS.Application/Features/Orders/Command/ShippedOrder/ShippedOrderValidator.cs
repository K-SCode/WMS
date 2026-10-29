using FluentValidation;

namespace WMS.Application.Features.Orders.Command.ShippedOrder
{
    public class ShippedOrderValidator : AbstractValidator<ShippedOrderCommand>
    {
        public ShippedOrderValidator() 
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);
        }
    }
}
