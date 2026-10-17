using WMS.Core.Common.Enum;

namespace WMS.Core.Entities
{
    public class GoodsReceiptsItems
    {
        public int Id { get; set; }
        public int GoodsReceiptID { get; set; }
        public GoodsReceipt GoodsReceipt { get; set; } = null!;

        public string Barcode { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string UnitQuantity { get; set; } = null!;
        public decimal PurchasePrice { get; set; }
        public ItemGrnStatus Status { get; set; }
    }
}
