namespace WMS.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? Nip { get; set; }
        public string? CompanyName { get; set; }

        public ICollection<Order> Orders { get; set; } = [];
    }
}
