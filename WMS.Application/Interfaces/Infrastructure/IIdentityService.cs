using WMS.Core.Common;
using WMS.Core.Entities.Auth;

namespace WMS.Application.Interfaces.Infrastructure
{
    public interface IIdentityService
    {
        Task<Result<bool>> RegisterUserAsync(
            string firstName,
            string lastName,
            string password,
            string email,
            string phone);
        Task<Result<AuthToken>> LoginUserAsync(string email, string password);
        Task<Result<bool>> AssignRole(string email, string role);
    }
}
