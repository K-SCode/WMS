using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;
using Dapper;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed class ListProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<ListProductQuery, Result<IEnumerable<ProductDto>>>
    {
        private const string sql = """
            SELECT
                Id,
                Name,
                Price,
                Barcode,
                UnitQuantity
            FROM [Products]
            """;
        public async ValueTask<Result<IEnumerable<ProductDto>>> Handle(
            ListProductQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var products = await connection.QueryAsync<ProductDto>(command);

            return products.ToList();
        }
    }
}
