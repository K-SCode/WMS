
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Orders.Dto
{
    public record OrderDto(
        int Id,
        string InvoiceNumber,
        OrderStatus Status,
        string UserEmail,
        string FirstName,
        string LastName,
        string Phone);
}
