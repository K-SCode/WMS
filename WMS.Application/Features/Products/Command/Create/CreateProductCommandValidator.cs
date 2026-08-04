using FluentValidation;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IUnitOfWorks unitOfWorks)
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

            RuleFor(product => product.Barcode)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (barcode, cancellationToken) =>
                    await unitOfWorks.Product.GetByBarcodeAsync(barcode,cancellationToken) is null)
                .WithMessage("Product with barcode: '{PropertyValue}' already exists");
        }
    }
}