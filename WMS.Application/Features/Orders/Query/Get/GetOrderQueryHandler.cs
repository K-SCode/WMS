using Dapper;
using Mapster;
using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Query.Get
{
    internal class GetOrderQueryHandler(
        ISqlConnectionFactory connectionFactory
        ) : IQueryHandler<GetOrderQuery, Result<OrderDetailsDto>>
    {
        public const string sql = """
            SELECT
                o.Id, 
                o.Status, 
                o.PaymentMethod, 
                o.Currency,
                o.ShippingAddress,
                o.InvoiceNumber,
                o.CreatedAt,
                o.UpdatedAt,
                c.FirstName,
                c.LastName,
                c.Phone,
                c.Email,
                c.Nip,
                c.CompanyName
                FROM [Order] o
                JOIN [Customer] c ON c.Id = o.CustomerId
                Where o.Id = @id;

                SELECT 
                    p.Name As ProductName,
                    i.Quantity,
                    i.UnitQuantity,
                    i.Price
                FROM [OrderItem] i
                JOIN [Products] p ON p.Id = i.ProductId
                WHERE i.OrderId = @id;
            """;
        public async ValueTask<Result<OrderDetailsDto>> Handle(
            GetOrderQuery query,
            CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                sql,
                new { query.Id },
                cancellationToken: cancellationToken);

            using var connecttion = connectionFactory.CreateConnection();
            var multi = await connecttion.QueryMultipleAsync(command);

            var header = await multi.ReadSingleOrDefaultAsync<OrderHeaderRow>();
            if (header is null)
                return Errors<Order>.ObjectNotExists(query.Id);

            var items = (await multi.ReadAsync<OrderItemDto>()).AsList();
            ConfigAdapter();

            var result = header.Adapt<OrderDetailsDto>() with { Items = items };
            return result;

        }

        private static void ConfigAdapter()
        {
            TypeAdapterConfig<OrderHeaderRow, OrderDetailsDto>
                .NewConfig()
                .Map(
                    d => d.Customer, s => new CustomerDto(
                        s.FirstName,
                        s.LastName,
                        s.Phone,
                        s.Email,
                        s.Nip,
                        s.CompanyName)
                    )
                .Ignore(d => d.Items);
        }

        private sealed class OrderHeaderRow
        {
            public int Id { get; set; }
            public string Status {get;set;}
            public string PaymentMethod {get;set;}
            public string Currency {get;set;}
            public string ShippingAddress {get;set;}
            public string? InvoiceNumber {get;set;}
            public DateTimeOffset CreatedAt {get;set;}
            public DateTimeOffset? UpdatedAt {get;set;}
            public string FirstName {get;set;}
            public string LastName {get;set;}
            public string Phone {get;set;}
            public string Email {get;set;}
            public string? Nip {get;set;}
            public string? CompanyName {get;set;}
            }
    }
}
