using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Products.Command.Update
{
    public sealed record UpdateProductCommand(int Id, string Name, decimal Price) : IRequest<bool>;
}
