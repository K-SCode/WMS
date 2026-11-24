using Dapper;
using Mediator;
using WMS.Application.Features.Raports.Dto;
using WMS.Application.Features.Raports.Query.GetMostProfitable;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Raports.Query.ListMostProfitable
{
    public class ListMostProfitableQueryHandler(
        ISqlConnectionFactory connectionFactory)
        : IQueryHandler<ListMostProfitableQuery, Result<IReadOnlyCollection<MostProfitableDto>>>
    {
        public const string Sql = $"""
            SELECT
                CAST(o.CreatedAt AS date) AS Day,
                p.Id AS ProductId,
                p.Name,
                o.Currency,
                SUM(i.Quantity) AS Quantity,
                SUM(i.Price * i.Quantity) AS Revenue,
                COUNT(DISTINCT o.Id) AS OrdersCount
            FROM [Order] o
            JOIN [OrderItem] i ON i.OrderId = o.Id
            JOIN [Products] p ON p.Id = i.ProductId
            WHERE o.CreatedAt >= @From
                AND o.CreatedAt <@To
                AND o.Status NOT in ('{nameof(OrderStatus.Cancelled)}','{nameof(OrderStatus.Returned)}')
            GROUP BY CAST(o.CreatedAt AS Date), p.Id, p.Name, o.Currency;
            """;
        public async ValueTask<Result<IReadOnlyCollection<MostProfitableDto>>> Handle(
            ListMostProfitableQuery query,
            CancellationToken cancellationToken)
        {
            IEnumerable<GetDailyDetails> days = await GetDailyDetailsAsync(query, cancellationToken);

            var report = GetReport(query, days);

            return report;
        }

        private List<MostProfitableDto> GetReport(ListMostProfitableQuery query, IEnumerable<GetDailyDetails> days) =>
            days
                .GroupBy(day => new
                {
                    PeriodStart = PeriodStart(day.Day, query.Peroid, query.FromTime.Date),
                    day.ProductId,
                    day.Currency,
                    day.Name
                }).Select(g => new MostProfitableDto(
                    DateOnly.FromDateTime(g.Key.PeriodStart),
                    g.Key.ProductId,
                    g.Key.Name,
                    g.Key.Currency,
                    g.Sum(day => day.Quantity),
                    Math.Round(g.Sum(day => day.Revenue), 2),
                    g.Sum(day => day.OrdersCount)
                    ))
                .OrderBy(row => row.PeriodStart)
                .ThenByDescending(row => row.Revenue)
                .ToList();

        private DateTime PeriodStart(DateTime day, ReportPeriod peroid, DateTime from)
            => peroid switch {
                ReportPeriod.Week => day.AddDays(-(((int)day.DayOfWeek + 6) % 7)),
                ReportPeriod.Month => new DateTime(day.Year, day.Month, 1),
                _ => from
            };

        private async Task<IEnumerable<GetDailyDetails>> GetDailyDetailsAsync(
            ListMostProfitableQuery query,
            CancellationToken ct)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(
                Sql,
                new
                {
                    from = query.FromTime,
                    to = query.ToTime
                },
                cancellationToken: ct
                );
            return await connection.QueryAsync<GetDailyDetails>(command);
        }

        private record GetDailyDetails(
            DateTime Day,
            int ProductId,
            string Name,
            string Currency,
            decimal Quantity,
            decimal Revenue,
            int OrdersCount
            );
    }
}
