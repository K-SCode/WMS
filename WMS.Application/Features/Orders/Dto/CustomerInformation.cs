namespace WMS.Application.Features.Orders.Dto
{
    public record CustomerInformation(
        string FirstName,
        string LastName,
        string Phone,
        string? Nip,
        string? CompanyName,
        string Email);
}
