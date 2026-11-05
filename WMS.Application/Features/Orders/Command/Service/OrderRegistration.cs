using Mapster;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class OrderRegistration(IUnitOfWork unitOfWorks) : IOrderRegistration
    {
        public async Task<Order> RegisterAsync(Order request, Customer customer, CancellationToken ct)
        {
            Order order = request.Adapt<Order>();
            order.Customer = customer;
            order.Status = OrderStatus.New;
            order.CreatedAt = DateTimeOffset.UtcNow;
            order.UpdatedAt = DateTimeOffset.UtcNow;
            order.InvoiceNumber = $"FV-{Guid.NewGuid().ToString("N")[..13].ToUpper()}";

            await unitOfWorks.Order.AddAsync(order, ct);
            return order;
        }
    }
}

