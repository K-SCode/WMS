using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed record UpdateLocationCommand(int Id, char Sector, int Rack, int Number) : ICommand<Result<bool>>;
}
