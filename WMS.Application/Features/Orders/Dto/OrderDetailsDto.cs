namespace WMS.Application.Features.Orders.Dto
{
    public record OrderDetailsDto(
        int Id,
        string Status,
        string ShippingAddress,
        string? InvoiceNumber,
        string PaymentMethod,
        string Currency,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt,
        CustomerDto Customer,
        IReadOnlyCollection<OrderItemDto> Items
        );
}
