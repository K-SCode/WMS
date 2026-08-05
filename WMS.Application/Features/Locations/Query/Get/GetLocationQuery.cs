using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Locations.Dtos;

namespace WMS.Application.Features.Locations.Query.Get
{
    public sealed record GetLocationQuery(int Id) : IQuery<LocationDto?>;

}
