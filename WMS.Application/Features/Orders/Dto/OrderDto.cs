using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Orders.Dto
{
    public sealed record OrderDto(
        string InvoiceNumber,
        Currency Currency,
        OrderStatus Status,
        string UserEmail,
        string FirstName,
        string LastName,
        string Phone);
}
