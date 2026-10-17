using WMS.Core.Common.Enum;

namespace WMS.Core.Entities
{
    public class GoodsReceipt
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public string GRNNumber { get; set; } = null!;
        public DateTime DateOfIssue { get; set; }
        public DateTime ReceivedAtUtc { get; set; }

        public string SupplierNip { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;

        public ICollection<GoodsReceiptsItems> Item { get; set; } = [];


        public OrderStatus Status { get; set; }
        public string Comment { get; set; } = null!;

    }
}
