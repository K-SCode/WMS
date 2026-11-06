using Shouldly;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.Core
{
    public class EntityWarehouseTests
    {
        [Theory]
        [InlineData(StockManageStatus.Accepted, WarehouseStatus.Pending)]
        [InlineData(StockManageStatus.PartiallyAccepted, WarehouseStatus.InAnalysis)]
        [InlineData(StockManageStatus.OutOfSapce, WarehouseStatus.InAnalysis)]
        [InlineData(StockManageStatus.ProductNotFound, WarehouseStatus.InAnalysis)]
        public void Constructor_StockStatus_DetermineTaskStatus(
            StockManageStatus stockStatus,
            WarehouseStatus expectedTaskStatus)
        {
            var task = new WarehouseTask(
                "1231231231231",
                30m,
                1,
                WarehouseSource.NewOrder,
                stockStatus,
                null);

            task.Status.ShouldBe(expectedTaskStatus);
        }
        [Theory]
        [InlineData(WarehouseSource.GoodsReceiptOverflow, 1, null)]
        [InlineData(WarehouseSource.NewOrder, null, 1)]
        [InlineData(WarehouseSource.OrderCancellation, null, 1)]
        [InlineData(WarehouseSource.CustomerReturn, null, 1)]
        public void Constructor_Source_DetetminesWhichDocumentIdsSet(
            WarehouseSource source,
            int? exceptedGoodsReceiptId,
            int? exceptedOrderId)
        {
            var task = new WarehouseTask(
                "1231231231231",
                30m,
                1,
                source,
                StockManageStatus.Accepted,
                1);

            task.GoodsReceiptId.ShouldBe(exceptedGoodsReceiptId);
            task.OrderId.ShouldBe(exceptedOrderId);
        }
    }
}
