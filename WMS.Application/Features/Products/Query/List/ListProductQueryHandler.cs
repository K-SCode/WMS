using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;
using Dapper;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed class ListProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<ListProductQuery, Result<IEnumerable<ProductDto>>>
    {
        private const string sql = "SELECT Id, Name, Price, Barcode, UnitQuantity FROM Products";
        public async ValueTask<Result<IEnumerable<ProductDto>>> Handle(
            ListProductQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var products = await connection.QueryAsync<ProductDto>(sql);
            return products.ToList();
        }
    }
}
