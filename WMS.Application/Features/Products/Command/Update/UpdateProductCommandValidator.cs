using FluentValidation;

namespace WMS.Application.Features.Products.Command.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);

            RuleFor(p => p.Name)
                .Length(3, 255);

            RuleFor(p => p.Price)
                .PrecisionScale(18, 2, true)
                .GreaterThan(0);

            RuleFor(p => p.UnitQuantity)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}
