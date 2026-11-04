using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public class CreateStockCommandValidator : AbstractValidator<CreateStockCommand>
    {
        public CreateStockCommandValidator()
        {
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
