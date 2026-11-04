using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed record CreateProductCommand(
        string Name,
        decimal Price,
        string UnitQuantity,
        string Barcode) : ICommand<Result<int>>;
}
