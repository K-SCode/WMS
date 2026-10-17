using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Orders.Dto
{
    public sealed record OrderInformation(
        string ShippingAddress,
        PaymentMethod PaymentMethod,
        Currency Currency);
}
