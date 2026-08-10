using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Update
{
    public sealed record UpdateProductCommand(int Id, string Name, decimal Price) : ICommand<Result<bool>>;
}
