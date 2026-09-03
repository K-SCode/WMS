using FluentValidation;

namespace WMS.Application.Features.GoodsReceived.Commands
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
