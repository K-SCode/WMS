using Dapper;
using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Locations.Query.List
{
    public sealed class ListLocationQueryHandler(
        ISqlConnectionFactory connectionFactory)
        : IQueryHandler<ListLocationQuery, Result<IReadOnlyCollection<LocationDto>>>
    {
        private const string sql = """
            SELECT
                Id,
                Sector,
                Rack,
                Number,
            FROM [Locations]
            """;
        public async ValueTask<Result<IReadOnlyCollection<LocationDto>>> Handle(
            ListLocationQuery query,
            CancellationToken cancellationToken)
        {
            var locations = await GetLocationsAsync(cancellationToken);
            return locations.AsList();
        }
        private async Task<IReadOnlyCollection<LocationDto>> GetLocationsAsync(CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, ct);
            var locations = await connection.QueryAsync<LocationDto>(command);
            return locations.AsList();
        }
    }
}
