using Mapster;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    internal class CreateOrderProcess(
        IAssignOrderUser assignOrderUser,
        IOrderRegistration orderRegistration,
        IAssignOrderItems assignOrderItems,
        IDecreaseQuantity decreaseQuantity,
        ICreateWarehouseTask createWarehouseTask,
        IUnitOfWorks unitOfWorks) : ICreateOrderProcess
    {
        public async Task<int> ProcessAsync(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> items,
            OrderInformation information,
            CancellationToken ct)
        {
            using var transaction = await unitOfWorks.BeginTransactionAsync(ct);
            Customer assignedCustomer = await assignOrderUser.AssignAsync(customer.Adapt<Customer>(), ct);

            Order registeredOrder = 
                await orderRegistration.RegisterAsync(
                information.Adapt<Order>(),
                assignedCustomer,
                ct);

            await unitOfWorks.SaveChangesAsync(ct);

            await assignOrderItems.Assing([.. items.Select( p => p.Adapt<OrderItem>())], registeredOrder,ct);

            var productsInOrder = await unitOfWorks.Product.GetAllByIdsAsync(items.Select(i => i.ProductId).ToList(),ct);

            List<(Product? Product, decimal Quantity)> products = new();
            foreach(var item in items)
            {
               products.Add((productsInOrder.FirstOrDefault(p => p.Id == item.ProductId), item.Quantity));
            }

            if (products.Count != items.Count || products.Any(p => p.Product is null))
                return -1;
            
            var result = await decreaseQuantity.DecreaseAsync(products, p => p.Product!.Barcode, p => p.Quantity, ct);

            if (result.Any(r => r.ResolveStatuses.Any(s => s.Status != StockIncreaseStatus.Accepted)))
                return -1;

            await createWarehouseTask.CreateTaskAsync(result, WarehouseSource.NewOrder, registeredOrder.Id, ct);

            await unitOfWorks.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return registeredOrder.Id;
        }
    }
}

