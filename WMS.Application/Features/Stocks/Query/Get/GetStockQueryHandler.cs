using Dapper;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public sealed class GetStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetStockQuery, Result<StockDto>>
    {
        private const string sql = "SELECT Id,LocationId, ProductId, Quantity FROM Stocks WHERE Id = @Id";
        public async ValueTask<Result<StockDto>> Handle(GetStockQuery query, CancellationToken cancellationToken)
        {
            var stock = await GetStockAsync(query.Id);

            if (StockExists(stock))
                return Errors<Stock>.ObjectNotExists(query.Id);

            return stock!;
        }

        private async Task<StockDto?> GetStockAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var stock = await connection.QueryFirstOrDefaultAsync<StockDto>(
                sql,
                new { Id = id });
            return stock;
        }
        private bool StockExists(StockDto? stock)
        {
            return stock is null;
        }
    }
}
