using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Query.List
{
    public sealed record ListLocationQuery : IQuery<Result<IReadOnlyCollection<LocationDto>>>;
}
