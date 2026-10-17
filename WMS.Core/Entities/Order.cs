using WMS.Core.Common.Enum;

namespace WMS.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? InvoiceNumber { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public Currency Currency {  get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt {  get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];

    }
}
