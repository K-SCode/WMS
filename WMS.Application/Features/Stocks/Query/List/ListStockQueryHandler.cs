using Dapper;
using Mediator;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Query.List
{
    public class ListStockQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<ListStockQuery, Result<IReadOnlyCollection<StockDto>>>
    {
        private const string sql = """
            SELECT
                Id,
                LocationId,
                ProductId,
                Quantity,
                MaxQuantity
                FROM [Stocks]
            """;
        public async ValueTask<Result<IReadOnlyCollection<StockDto>>> Handle(ListStockQuery query, CancellationToken cancellationToken)
        {
            var stocks = await GetStocksDtoAsync(cancellationToken);
            return stocks.AsList();
        }

        private async Task<IReadOnlyCollection<StockDto>> GetStocksDtoAsync(CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, ct);

            var stocks = await connection.QueryAsync<StockDto>(command);
            return stocks.AsList();
        }
    }
}
