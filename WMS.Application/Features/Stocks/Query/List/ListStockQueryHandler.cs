using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Query.List
{
    internal class ListStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<ListStockQuery, Result<IEnumerable<StockDto>>>
    {
        public async ValueTask<Result<IEnumerable<StockDto>>> Handle(ListStockQuery query, CancellationToken cancellationToken)
        {
            var stocks = await GetStocksDtoAsync();
            return stocks.ToList();
        }

        private async Task<IEnumerable<StockDto>> GetStocksDtoAsync()
        {
            using var connection = connectionFactory.CreateConnection();

            var sql = "SELECT Id,LocationId, ProductId, Quantity FROM Stocks";

            var stocks = await connection.QueryAsync<StockDto>(sql);
            return stocks;
        }
    }
}
