using Dapper;
using Mediator;
using WMS.Application.Features.WarehouseTasks.Dtos;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Features.WarehouseTasks.Query
{
    public class GetMyTasksQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ICurrentUser currentUser) : IQueryHandler<GetMyTasksQuery, Result<IReadOnlyCollection<WarehouseTaskDto>>>
    {
        private const string sql = """
            SELECT
                Id,
                Barcode,
                Quantity,
                Source,
                Status,
                OrderId,
                GoodsReceiptId,
                LocationId,
                CreatedAt,
                AssignedAt
            FROM[WarehouseTask]
            WHERE AssignedUserId = @UserId
              AND Status IN('Assigned', 'InProgress')
            ORDER BY CreatedAt
            """;

        public async ValueTask<Result<IReadOnlyCollection<WarehouseTaskDto>>> Handle(GetMyTasksQuery query, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var command = new CommandDefinition(
                sql,
                new { currentUser.UserId },
                cancellationToken: cancellationToken);

            var tasks = await connection.QueryAsync<WarehouseTaskDto>(command);
            return tasks.AsList();
        }
    }
}
