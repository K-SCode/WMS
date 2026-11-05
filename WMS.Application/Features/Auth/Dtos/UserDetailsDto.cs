namespace WMS.Application.Features.Auth.Dtos
{
    public sealed record UserDetailsDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Role,
        DateTimeOffset CratedAt,
        bool IsActive);
}
