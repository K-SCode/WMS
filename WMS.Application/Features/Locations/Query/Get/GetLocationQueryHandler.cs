using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Locations.Query.Get
{
    public class GetLocationQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<GetLocationQuery, LocationDto?>
    {
        public async ValueTask<LocationDto?> Handle(GetLocationQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var sql = $"SELECT Id, Sector, Rack, Number, IsFull FROM Locations WHERE Id = @Id";

            var location = await connection.QueryFirstOrDefaultAsync<LocationDto>(
                sql,
                new { Id = query.Id });
            return location;
        }
    }
}
