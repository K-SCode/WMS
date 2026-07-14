using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<int>;
}
