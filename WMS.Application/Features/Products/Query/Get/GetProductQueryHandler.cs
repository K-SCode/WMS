using Dapper;
using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Products.Query.Get
{
    public sealed class GetProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IRequestHandler<GetProductQuery, ProductDto?>
    {
        public async ValueTask<ProductDto?> Handle(
            GetProductQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var sql = $"SELECT Id, Name, Price FROM Products WHERE Id = @Id";

            var product = await connection.QueryFirstOrDefaultAsync<ProductDto>(
                sql,
                new {Id = request.Id});
            return product;
        }
    }
}
