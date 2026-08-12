using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Query.Get
{
    public sealed record GetLocationQuery(int Id) : IQuery<Result<LocationDto>>;

}
