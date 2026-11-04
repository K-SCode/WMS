using Dapper;
using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WMS.Application.Features.Products.Query.Get
{
    public sealed class GetProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<GetProductQuery, Result<ProductDto>>
    {
        private const string sql = """
            SELECT
                Id,
                Name,
                Price,
                Barcode,
                UnitQuantity
            FROM [Products]
            WHERE Id = @Id
            """;
        public async ValueTask<Result<ProductDto>> Handle(
            GetProductQuery query,
            CancellationToken cancellationToken)
        {

            var product = await GetProductAsync(query.Id, cancellationToken);

            if (!ProductExists(product))
                return Errors<Product>.ObjectNotExists(query.Id);

            return product!;
        }
        
        private async Task<ProductDto?> GetProductAsync(int id, CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(
                sql,
                new { id },
                cancellationToken: ct);
            var product =
                await connection.QueryFirstOrDefaultAsync<ProductDto>(command);

            return product;
        }
        private bool ProductExists(ProductDto? product) =>
            product is not null;
    }
}
