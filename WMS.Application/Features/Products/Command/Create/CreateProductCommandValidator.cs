using FluentValidation;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .Length(3, 255);

            RuleFor(p => p.Price)
                .PrecisionScale(18, 2, true)
                .GreaterThan(0);

            RuleFor(p => p.Barcode)
                .Length(13)
                .Matches(@"^\d+$");

            RuleFor(p => p.UnitQuantity)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}