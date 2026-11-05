using Dapper;
using Mediator;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public sealed class GetStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetStockQuery, Result<StockDto>>
    {
        private const string sql = """
            SELECT
                Id,
                LocationId,
                ProductId,
                Quantity
            FROM [Stocks]
            WHERE Id = @Id
            """;
        public async ValueTask<Result<StockDto>> Handle(GetStockQuery query, CancellationToken cancellationToken)
        {
            var stock = await GetStockAsync(query.Id,cancellationToken);

            if (StockExists(stock))
                return Errors<Stock>.ObjectNotExists(query.Id);

            return stock!;
        }

        private async Task<StockDto?> GetStockAsync(int id, CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: ct);
            var stock = await connection.QueryFirstOrDefaultAsync<StockDto>(command);
            return stock;
        }
        private bool StockExists(StockDto? stock)
        {
            return stock is null;
        }
    }
}
