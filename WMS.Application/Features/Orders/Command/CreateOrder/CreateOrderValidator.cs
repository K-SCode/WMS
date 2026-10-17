using FluentValidation;
using WMS.Application.Features.Orders.Command.ValidatorsHelper;

namespace WMS.Application.Features.Orders.Command.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(p => p.Customer)
                .NotNull()
                .SetValidator(new CustomerInformationValidator());
            RuleFor(p => p.Products)
                .NotEmpty()
                .Must(p => p.Select(x => x.ProductId).Distinct().Count() == p.Count)
                .WithMessage("Products cannot be duplicated");

            RuleForEach(p => p.Products)
                .NotNull()
                .SetValidator(new CreateProductOrderValidator());
            RuleFor(p => p.Information)
                .NotNull()
                .SetValidator(new OrderInformationValidator());
                

        }
    }
}
