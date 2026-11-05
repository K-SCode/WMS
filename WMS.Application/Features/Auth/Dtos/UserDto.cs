namespace WMS.Application.Features.Auth.Dtos
{
    public sealed record UserDto(int Id, string Email, string Phone, string Role, string FirstName, string LastName);
}
