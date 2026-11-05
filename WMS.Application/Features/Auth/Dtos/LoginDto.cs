namespace WMS.Application.Features.Auth.Dtos
{
    public sealed record LoginDto(string Token, DateTime ExpiresAt);
}
