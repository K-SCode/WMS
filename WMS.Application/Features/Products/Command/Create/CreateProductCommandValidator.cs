using FluentValidation;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .Length(3, 255);


            RuleFor(product => product.Price)
                .NotEmpty()
                .PrecisionScale(18, 2, true)
                .GreaterThan(0);

            RuleFor(product => product.Barcode)
                .NotEmpty()
                .Length(13);
        }
    }
}