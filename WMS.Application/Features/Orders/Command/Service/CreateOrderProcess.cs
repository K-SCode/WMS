using Mapster;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    internal class CreateOrderProcess(
        IAssignOrderUser assignOrderUser,
        IOrderRegistration orderRegistration,
        IAssignOrderItems assignOrderItems,
        IUnitOfWorks unitOfWorks) : ICreateOrderProcess
    {
        public async Task<int> ProcessAsync(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> items,
            OrderInformation information,
            CancellationToken ct)
        {
            Customer assignedCustomer = await assignOrderUser.AssignAsync(customer.Adapt<Customer>(), ct);

            Order registeredOrder = 
                await orderRegistration.RegisterAsync(
                information.Adapt<Order>(),
                assignedCustomer,
                ct);

            await assignOrderItems.Assing([.. items.Select( p => p.Adapt<OrderItem>())], registeredOrder,ct);
            await unitOfWorks.SaveChangesAsync(ct);
            return registeredOrder.Id;
        }
    }
}

