using WMS.Core.Common;
using WMS.Core.Entities.Auth;

namespace WMS.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<bool>> RegisterUserAsync(
            string firstName,
            string lastName,
            string password,
            string email,
            string phone,
            CancellationToken ct);
        Task<Result<AuthToken>> LoginUserAsync(string email, string password, CancellationToken ct);
    }
}
