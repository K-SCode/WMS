using FluentValidation;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(stock => stock.LocationId)
                .GreaterThan(0);
            RuleFor(stock => stock.ProductId)
                .GreaterThan(0);

            RuleFor(stock => stock.Quantity)
                .GreaterThan(0)
                .LessThan(int.MaxValue);
            RuleFor(stock => stock.MaxQuantity)
                .GreaterThanOrEqualTo(stock => stock.Quantity);
        }
    }
}
