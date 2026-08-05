using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Locations.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Query.List
{
    public sealed class ListLocationQueryHandler(
        ISqlConnectionFactory connectionFactory)
        : IQueryHandler<ListLocationQuery, IEnumerable<LocationDto?>>
    {
        public async ValueTask<IEnumerable<LocationDto?>> Handle(
            ListLocationQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var sql = $"SELECT Id, Sector, Rack, Number, IsFull FROM Locations";

            var locations = await connection.QueryAsync<LocationDto>(sql);
            return locations;
        }
    }
}
