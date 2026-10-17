using Dapper;
using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Query.Get
{
    public sealed class GetProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<GetProductQuery, Result<ProductDto>>
    {
        private const string sql = "SELECT Id, Name, Price, Barcode, UnitQuantity FROM Products WHERE Id = @Id";
        public async ValueTask<Result<ProductDto>> Handle(
            GetProductQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var product = await connection.QueryFirstOrDefaultAsync<ProductDto>(
                sql,
                new {Id = query.Id});

            if (product is null)
                return Errors<Product>.ObjectNotExists(query.Id);

            return product;
        }
    }
}
