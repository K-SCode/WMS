namespace WMS.Application.Features.Orders.Dto
{
    public record OrderDto(
        int Id,
        string InvoiceNumber,
       string Status,
        string UserEmail,
        string FirstName,
        string LastName,
        string Phone);
}
