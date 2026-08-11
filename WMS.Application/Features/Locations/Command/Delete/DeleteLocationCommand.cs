using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Command.Delete
{
    public sealed record DeleteLocationCommand(int Id) : ICommand<Result<bool>>;
}
