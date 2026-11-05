using Dapper;
using Mapster;
using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Query.Get
{
    public class GetGoodsReceivedQueryHandler(
        ISqlConnectionFactory connectionFactory)
        : IQueryHandler<GetGoodsReceivedQuery, Result<GoodsReceivedDetailsDto>>
    {
        private const string Sql = """
            SELECT
                g.Id,
                g.InvoiceNumber,
                g.GRNNumber,
                g.DateOfIssue,
                g.ReceivedAtUtc,
                g.SupplierNip,
                g.Status
                g.Comment
            FROM [GoodsReceipt] g
            WHERE g.Id = @id

            SELECT 
                i.Barcode,
                i.Quantity,
                i.UnitQuantity,
                i.PurchasePrice
            FROM [GoodsReceiptsItems] i
            WHERE i.GoodsReceiptID = @id
            """;
        public async ValueTask<Result<GoodsReceivedDetailsDto>> Handle(
            GetGoodsReceivedQuery query,
            CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                Sql,
                new { id = query.Id },
                cancellationToken: cancellationToken);

            using var connection = connectionFactory.CreateConnection();
            var multi = await connection.QueryMultipleAsync(command);

            var header = await multi.ReadSingleOrDefaultAsync<GoodsReceivedHeaderRow>();
            if (header is null)
                return Errors<GoodsReceipt>.ObjectNotExists(query.Id);

            var items = (await multi.ReadAsync<GoodsReceivedItemsDto>()).AsList();

            var result = header.Adapt<GoodsReceivedDetailsDto>() with { Items = items };
            return result;
        }

        private class GoodsReceivedHeaderRow
        {
            public int Id { get; set; }
            public string InvoiceNumber { get; set; } = null!;
            public string GRNNumber {get;set;} = null!;
            public DateTimeOffset DateOfIssue {get;set;}
            public DateTimeOffset ReceivedAtUtc {get;set;}
            public string SupplierNip {get;set;} = null!;
            public string Status {get;set;} = null!;
            public string Comment {get;set;} = null!;
            public string Barcode {get;set;} = null!;
            public decimal Quantity {get;set;}
            public string UnitQuantity {get;set;} = null!;
            public decimal PurchasePrice {get;set;}
        }
    }
}
