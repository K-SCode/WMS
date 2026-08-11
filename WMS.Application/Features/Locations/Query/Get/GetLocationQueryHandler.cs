using Dapper;
using Mediator;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Query.Get
{
    public class GetLocationQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<GetLocationQuery, Result<LocationDto?>>
    {
        private const string sql = $"SELECT Id, Sector, Rack, Number, IsFull FROM Locations WHERE Id = @Id";
        public async ValueTask<Result<LocationDto?>> Handle(GetLocationQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var location = await connection.QueryFirstOrDefaultAsync<LocationDto>(
                sql,
                new { Id = query.Id });

            if(location is null)
                return Errors<Location>.ObjectNotExists(query.Id);

            return location;
        }
    }
}
