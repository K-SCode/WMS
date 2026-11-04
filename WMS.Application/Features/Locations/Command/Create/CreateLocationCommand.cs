using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Command.Create
{
    public sealed record CreateLocationCommand(char Sector, int Rack, int Number) : ICommand<Result<int>>;
}
