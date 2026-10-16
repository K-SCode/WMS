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

        public int SupplierId { get; set;  }
        public Supplier Supplier { get; set; }

        public ICollection<GoodsReceiptsItems> Item { get; set; } = new List<GoodsReceiptsItems>();


        public OrderStatus Status { get; set; }
        public string Comment { get; set; } = null!;

    }
}
