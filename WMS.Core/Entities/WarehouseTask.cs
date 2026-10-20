using WMS.Core.Common.Enum;

namespace WMS.Core.Entities
{
    public class WarehouseTask
    {
        private WarehouseTask() { }
        public WarehouseTask(
            string barcode,
            decimal quantity,
            int documentId,
            WarehouseSource source,
            StockIncreaseStatus stockStatus,
            int? locationId)
        {
            Barcode = barcode;
            Quantity = quantity;
            Source = source;

            if (source == WarehouseSource.GoodsReceiptOverflow)
                GoodsReceipId = documentId;
            else
                OrderId = documentId;

            StockStatus = stockStatus;
            Status = stockStatus != StockIncreaseStatus.Accepted 
                ? WarehouseStatus.InAnalyssis 
                : WarehouseStatus.Pending;

            CreatedAt = DateTime.UtcNow;
            LocationId = locationId;
        }

        public int Id { get; set; }
        public string Barcode { get; set; } = null!;
        public decimal Quantity { get; set; }

        public int? GoodsReceipId { get; set; }
        public GoodsReceipt? GoodsReceipt { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }


        public WarehouseSource Source { get; set; }
        public WarehouseStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; } = null!;
        public StockIncreaseStatus StockStatus { get; set; }

    }
    public enum WarehouseSource
    {
        GoodsReceiptOverflow,
        OrderCancellation,
        CustomerReturn
    }
    public enum WarehouseStatus
    {
        Pending,
        InAnalyssis,
        Completed
    }
}
