namespace WMS.Core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string NIP { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
