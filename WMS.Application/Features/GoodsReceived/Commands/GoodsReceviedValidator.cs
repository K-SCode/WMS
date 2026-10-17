using FluentValidation;

namespace WMS.Application.Features.GoodsReceived.Commands
{
    public class GoodsReceviedValidator : AbstractValidator<ProductReceived>
    {
        public GoodsReceviedValidator()
        {
            RuleFor(property => property.Quantity)
                .PrecisionScale(18, 3, true)
                .GreaterThan(0);
            RuleFor(property => property.Barcode)
                .Length(13);
            RuleFor(property => property.PurchasePrice)
                .PrecisionScale(18, 2, true)
                .GreaterThan(0);

            RuleFor(property => property.UnitQuantity)
                .NotEmpty()
                .NotNull();

        }
    }
}
