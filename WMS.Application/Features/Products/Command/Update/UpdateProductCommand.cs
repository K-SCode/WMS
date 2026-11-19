using Mediator;
using WMS.Application.common.Abstraction;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Update
{
    public sealed record UpdateProductCommand(
        int Id,
        string Name,
        decimal Price,
        string UnitQuantity,
        byte[] Version) : ICommand<Result<bool>>, IVersionedCommand;
}
