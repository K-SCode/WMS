
namespace WMS.Application.Features.Orders.Dto
{
    public record OrderItemDto
    (
        string ProductName,
        decimal Quantity,
        string UnitQuantity,
        decimal Price
    );
}
