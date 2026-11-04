using Dapper;
using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Query.Get
{
    public class GetLocationQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<GetLocationQuery, Result<LocationDto>>
    {
        private const string sql = """
            SELECT
                Id,
                Sector,
                Rack,
                Number,
            FROM [Locations]
            WHERE Id = @Id
            """;
        public async ValueTask<Result<LocationDto>> Handle(GetLocationQuery query, CancellationToken cancellationToken)
        {
            var location = await GetLocationAsync(query.Id, cancellationToken);

            if(!LocationExists(location))
                return Errors<Location>.ObjectNotExists(query.Id);

            return location!;
        }
        private async Task<LocationDto?> GetLocationAsync(int id, CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, new { id }, cancellationToken: ct);
            var location = await connection.QueryFirstOrDefaultAsync<LocationDto>(command);
            return location;
        }

        private static bool LocationExists(LocationDto? location) =>
            location is not null;
    }
}
