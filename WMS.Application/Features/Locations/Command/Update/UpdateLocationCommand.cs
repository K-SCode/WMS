using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed record UpdateLocationCommand(int Id, char Sector, int Rack, int Number, bool IsFull) : ICommand<Result<bool>>;
}
