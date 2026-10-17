using Mapster;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class OrderRegistration(IUnitOfWorks unitOfWorks) : IOrderRegistration
    {
        public async Task<Order> RegisterAsync(Order request, Customer customer, CancellationToken ct)
        {
            Order order = request.Adapt<Order>();
            order.Customer = customer;
            order.Status = OrderStatus.New;
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            order.InvoiceNumber = $"FV-{Guid.NewGuid().ToString("N")[..13].ToUpper()}";

            await unitOfWorks.Order.AddAsync(order, ct);
            return order;
        }
    }
}

