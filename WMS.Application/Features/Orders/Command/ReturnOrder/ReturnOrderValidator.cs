
using FluentValidation;

namespace WMS.Application.Features.Orders.Command.ReturnOrder
{
    public class ReturnOrderValidator : AbstractValidator<ReturnOrderCommand>
    {
        public ReturnOrderValidator() {
            RuleFor(command => command.Id)
                .GreaterThan(0);
        }
    }
}
