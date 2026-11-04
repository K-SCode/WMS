using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Delete
{
    public sealed record DeleteProductCommand(int Id) : ICommand<Result<bool>>;
}
