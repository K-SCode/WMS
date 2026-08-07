using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public sealed class GetStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetStockQuery, StockDto?>
    {
        public async ValueTask<StockDto?> Handle(GetStockQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var sql = "SELECT Id,LocationId, ProductId, Quantity FROM Stocks WHERE Id = @Id";
            var stock = await connection.QueryFirstOrDefaultAsync<StockDto>(
                sql,
                new { Id = query.Id });
            return stock;
        }
    }
}
