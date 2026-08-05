using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Command.Delete
{
    public sealed record DeleteLocationCommand(int Id) : ICommand<bool>;
}
