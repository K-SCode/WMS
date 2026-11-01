using Microsoft.AspNetCore.Identity;

namespace WMS.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTimeOffset CratedAt { get; set; }
    }
}
