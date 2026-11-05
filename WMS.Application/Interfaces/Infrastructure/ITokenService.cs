using WMS.Core.Entities.Auth;

namespace WMS.Application.Interfaces.Infrastructure
{
    public interface ITokenService
    {
        Task<AuthToken> CreateTokenAsync(int userId);
    }
}
