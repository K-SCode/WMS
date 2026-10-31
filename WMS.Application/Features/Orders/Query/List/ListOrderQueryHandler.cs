using Dapper;
using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Query.List
{
    public class ListOrderQueryHandler(
        ISqlConnectionFactory connectionFactory) : IQueryHandler<ListOrderQuery, Result<IReadOnlyCollection<OrderDto>>>
    {
        private const string sql = """
            
                SELECT
                    o.Id,
                    o.InvoiceNumber, 
                    o.Status,
                    c.Email AS UserEmail,
                    c.FirstName,
                    c.LastName, 
                    c.Phone
                FROM [Order] o
                LEFT JOIN [Customer] c ON o.CustomerId = c.id
            
            """;
        public async ValueTask<Result<IReadOnlyCollection<OrderDto>>> Handle(ListOrderQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();

            var orders = await connection.QueryAsync<OrderDto>(sql);
            return orders.ToList();
        }
    }
}
