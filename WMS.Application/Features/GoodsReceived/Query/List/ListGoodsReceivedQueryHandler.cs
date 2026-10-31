using Dapper;
using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.GoodsReceived.Query.List
{
    public class ListGoodsReceivedQueryHandler(
        ISqlConnectionFactory connectionFactory
        )
        : IQueryHandler<ListGoodsReceivedQuery, Result<IReadOnlyCollection<GoodsReceivedDto>>>
    {
        private const string Sql = """
            SELECT
                g.Id,
                g.InvoiceNumber,
                g.GRNNumber,
                g.DateOfIssue,
                g.SupplierNip,
                g.Status
            FROM [GoodsReceipt] g
            """;
        public async ValueTask<Result<IReadOnlyCollection<GoodsReceivedDto>>> Handle(
            ListGoodsReceivedQuery query,
            CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(Sql, cancellationToken:  cancellationToken);
            using var connection = connectionFactory.CreateConnection();

            var products = await connection.QueryAsync<GoodsReceivedDto>(command);
            return products.AsList();

        }
    }
}
