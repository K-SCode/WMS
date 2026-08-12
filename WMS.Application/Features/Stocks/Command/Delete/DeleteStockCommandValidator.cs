using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Stocks.Command.Delete
{
    public class DeleteStockCommandValidator : AbstractValidator<DeleteStockCommand>
    {
        public DeleteStockCommandValidator() {
            RuleFor(stock => stock.Id)
                .GreaterThan(0);
        }
    }
}
