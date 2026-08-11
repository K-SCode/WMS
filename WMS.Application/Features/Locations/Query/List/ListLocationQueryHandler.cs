using Dapper;
using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Query.List
{
    public sealed class ListLocationQueryHandler(
        ISqlConnectionFactory connectionFactory)
        : IQueryHandler<ListLocationQuery, Result<IEnumerable<LocationDto>>>
    {
        private const string sql = $"SELECT Id, Sector, Rack, Number, IsFull FROM Locations";
        public async ValueTask<Result<IEnumerable<LocationDto>>> Handle(
            ListLocationQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var locations = await connection.QueryAsync<LocationDto>(sql);
            return locations.ToList();
        }
    }
}
