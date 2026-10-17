using FluentValidation;
using WMS.Application.Features.Orders.Dto;

namespace WMS.Application.Features.Orders.Command.ValidatorsHelper
{
    public class CreateProductOrderValidator : AbstractValidator<CreateProductOrder>
    {
        public CreateProductOrderValidator() 
        {
            RuleFor(p => p.ProductId)
                .GreaterThan(0);
            RuleFor(p => p.Quantity)
                .GreaterThan(0);
        }
    }
}
