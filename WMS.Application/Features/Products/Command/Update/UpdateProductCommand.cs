using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Update
{
    public sealed record UpdateProductCommand(int Id, string Name, decimal Price, List<Stock> Stocks) : IRequest<bool>;
}
