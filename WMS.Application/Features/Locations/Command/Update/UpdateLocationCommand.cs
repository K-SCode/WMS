using Mediator;
using WMS.Application.common.Abstraction;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed record UpdateLocationCommand(int Id, char Sector, int Rack, int Number, byte[] Version)
        : ICommand<Result<bool>>, IVersionedCommand;
}
