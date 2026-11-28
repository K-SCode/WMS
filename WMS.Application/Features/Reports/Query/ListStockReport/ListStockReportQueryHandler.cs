using Dapper;
using Mediator;
using WMS.Application.Features.Reports.Dto;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Reports.Query.ListStockReport
{
    public sealed class ListStockReportQueryHandler(ISqlConnectionFactory connectionFactory)
        : IQueryHandler<ListStockReportQuery, Result<IDictionary<string, List<ListStockReportDto>>>>
    {
        private const int UnSoldDays = 30;
        private const string Sql = $"""
            SELECT
                p.Id AS ProductId,
                p.Name,
                SUM(s.Quantity) AS Quantity,
                p.UnitQuantity AS Unit,
                CAST((SELECT MAX(o.CreatedAt)
                   FROM [OrderItem] i
                   JOIN [Order] o ON o.Id = i.OrderId
                  WHERE i.ProductId = p.Id
                    AND o.Status NOT IN ('{nameof(OrderStatus.Cancelled)}', '{nameof(OrderStatus.Returned)}')
                ) AS date)AS LastSoldAt
            FROM [Products] p
            JOIN [Stocks] s ON s.ProductId = p.Id
            WHERE p.Id NOT IN (
                SELECT i.ProductId
                FROM [OrderItem] i
                JOIN [Order] o ON o.Id = i.OrderId
                WHERE o.Status NOT IN ('{nameof(OrderStatus.Cancelled)}', '{nameof(OrderStatus.Returned)}')
                  AND o.CreatedAt >= DATEADD(day, -@Days, SYSDATETIMEOFFSET())
            )
            GROUP BY p.Id, p.Name, p.UnitQuantity
            HAVING SUM(s.Quantity) > 0
            ORDER BY Quantity DESC;
            """;

        public async ValueTask<Result<IDictionary<string,List<ListStockReportDto>>>> Handle(
            ListStockReportQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(Sql, new { Days = UnSoldDays },cancellationToken: cancellationToken);

            var items = await connection.QueryAsync<ListStockReportDto>(command);

            var report =
                items.GroupBy(
                    d => d.LastSoldAt is null ? "deadStock" : "stagnantProducts")
                .ToDictionary(g => g.Key, g => g.OrderBy(p => p.LastSoldAt).AsList());
            return report;
        }
    }
}
