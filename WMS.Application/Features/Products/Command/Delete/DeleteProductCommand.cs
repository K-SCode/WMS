using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Products.Command.Delete
{
    public sealed record DeleteProductCommand(int Id) : IRequest<bool>;
}
