
namespace WMS.Application.Features.Orders.Dto
{
    public record CustomerDto
    (
        string FirstName,
        string LastName,
        string Phone,
        string Email,
        string? Nip,
        string? CompanyName
    );
}
