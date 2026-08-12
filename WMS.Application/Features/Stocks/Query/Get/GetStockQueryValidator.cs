using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public class GetStockQueryValidator :AbstractValidator<GetStockQuery>
    {
        public GetStockQueryValidator() 
        {
            RuleFor(stock => stock.Id)
                .GreaterThan(0);
        }
    }
}
