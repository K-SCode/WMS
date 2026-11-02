using WMS.Core.Entities.Auth;

namespace WMS.Application.Interfaces
{
    public interface ITokenService
    {
        Task<AuthToken> CreateTokenAsync(int userId, CancellationToken ct);
    }
}
