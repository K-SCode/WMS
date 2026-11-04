using FluentValidation;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(stock => stock.LocationId)
                .GreaterThan(0);
            RuleFor(stock => stock.LocationId)
                .GreaterThan(0);
            RuleFor(stock => stock.ProductId)
                .GreaterThan(0);

            RuleFor(stock => stock.Quantity)
                .PrecisionScale(18, 2, true)
                .GreaterThan(0);

            RuleFor(stock => stock.MaxQuantity)
                .PrecisionScale(18, 2, true)
                .GreaterThan(0)
                .GreaterThanOrEqualTo(stock => stock.Quantity);
        }
    }
}
