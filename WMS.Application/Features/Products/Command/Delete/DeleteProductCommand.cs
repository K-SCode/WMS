using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Delete
{
    public sealed record DeleteProductCommand(int Id) : ICommand<Result<bool>>;
}
