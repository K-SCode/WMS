using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Locations.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Query.List
{
    public sealed record ListLocationQuery : IQuery<Result<IEnumerable<LocationDto>>>;
}
