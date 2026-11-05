using Mediator;
using WMS.Application.Features.Products.Dtos;
using Dapper;
using WMS.Core.Common;
using WMS.Application.Interfaces.Infrastructure;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed class ListProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<ListProductQuery, Result<IReadOnlyCollection<ProductDto>>>
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
        public async ValueTask<Result<IReadOnlyCollection<ProductDto>>> Handle(
            ListProductQuery query,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ProductDto> products = await GetProductsAsync(cancellationToken);

            return products.AsList();
        }

        private async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync(CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            var products = await connection.QueryAsync<ProductDto>(command);
            return products.AsList();
        }
    }
}
