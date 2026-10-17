namespace WMS.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public decimal Quantity {  get; set; }
        public string UnitQuantity { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
