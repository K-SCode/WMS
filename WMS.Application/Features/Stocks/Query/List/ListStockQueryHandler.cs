using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Stocks.Query.List
{
    internal class ListStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<ListStockQuery, IEnumerable<StockDto>?>
    {
        public async ValueTask<IEnumerable<StockDto>?> Handle(ListStockQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var sql = "SELECT Id,LocationId, ProductId, Quantity FROM Stocks";

            var stocks = await connection.QueryAsync<StockDto>(sql);

            return stocks;
        }
    }
}
