using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Command.Create
{
    public sealed record CreateLocationCommand(char Sector, int Rack, int Number, bool IsFull) : ICommand<int>;
}
