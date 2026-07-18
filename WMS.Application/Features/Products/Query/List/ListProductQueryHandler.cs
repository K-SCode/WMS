using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Dtos;
using Mapster;
using MapsterMapper;
using WMS.Application.Interfaces;
using Dapper;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed class ListProductQueryHandler(
        ISqlConnectionFactory connectionFactory) : IRequestHandler<ListProductQuery, List<ProductDto>>
    {
        public async ValueTask<List<ProductDto>> Handle(
            ListProductQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var sql = $"SELECT Id, Name, Price FROM Products";

            var product = await connection.QueryAsync<ProductDto>(sql);
            return product.ToList();
        }
    }
}
