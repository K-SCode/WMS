using FluentValidation;

namespace WMS.Application.Features.GoodsReceived
{
    public class GoodsReceviedValidator : AbstractValidator<ProductReceived>
    {
        public GoodsReceviedValidator()
        {
            RuleFor(property => property.Quantity)
                .GreaterThan(0);
            RuleFor(property => property.Barcode)
                .Length(13);
        }
    }
}
