using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed record CreateProductCommand(string Name, decimal Price, string Barcode) : ICommand<Result<int>>;
}
