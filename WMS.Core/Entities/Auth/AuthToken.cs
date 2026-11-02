namespace WMS.Core.Entities.Auth
{
    public sealed record AuthToken(
        string Token,
        DateTime ExpiresAt);
}
