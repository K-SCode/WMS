using WMS.Application.Interfaces.Infrastructure;

namespace WMS.Api.Services
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public int UserId
        {
            get
            {
                string? sub = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
                return int.TryParse(sub, out int id)
                    ? id
                    : throw new InvalidOperationException("The request has no authenticated user.");
            }
        }
    }
}
