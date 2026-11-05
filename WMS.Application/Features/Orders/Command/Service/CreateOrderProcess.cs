using Mapster;
using WMS.Application.common.Service;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Application.Interfaces.Stocks;
using WMS.Application.Interfaces.Warehouse;
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
        IUnitOfWork unitOfWorks) : ICreateOrderProcess
    {
        public async Task<Result<int>> ProcessAsync(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> items,
            OrderInformation information,
            CancellationToken ct)
        {
            using var transaction = await unitOfWorks.BeginTransactionAsync(ct);
            var result = await DecreaseStockForOrderAsync(items, ct);
            if (!result.IsSuccess)
            {
                return result.Error!;
            }

            Order order = await CreateOrderInformation(customer, information, ct);
            await unitOfWorks.SaveChangesAsync(ct);

            await AssignItemsToOrder(items, order, ct);
            await CreateTask(order, result.Value, ct);

            await unitOfWorks.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return order.Id;
        }

        private async Task<Result<IReadOnlyList<StockResult>>> DecreaseStockForOrderAsync(
            IReadOnlyCollection<CreateProductOrder> items,
            CancellationToken ct)
        {
            var products =
                await unitOfWorks.Product.GetAllByIdsAsync(
                    [.. items.Select(i => i.ProductId)],
                    ct);

            var lines = BuildOrderLine(items, products);

            if (!lines.IsSuccess)
                return  lines.Error!;

            IReadOnlyList<StockResult>  stockResult = await decreaseQuantity.DecreaseAsync(
                lines.Value, p => p.Product!.Barcode,
                p => p.Quantity, ct);
            if (HasUnacceptedStatus(stockResult))
                return Errors<Stock>.InsufficeintQuantity;
            return stockResult.ToList();
        }

        private async Task<Order> CreateOrderInformation(
            CustomerInformation customer,
            OrderInformation information,
            CancellationToken ct)
        {
            Customer assignedCustomer = await AssignCustomer(customer, ct);
            Order registeredOrder = await RegisterOrder(information, assignedCustomer, ct);
            return registeredOrder;
        }

        private Task CreateTask(
            Order registeredOrder,
            IReadOnlyList<StockResult> result,
            CancellationToken ct)
        {
            return createWarehouseTask.CreateTaskAsync(
                result,
                WarehouseSource.NewOrder,
                registeredOrder.Id,
                ct);
        }

        private static bool HasUnacceptedStatus(
            IReadOnlyList<StockResult> result)
        {
            return result.Any(
                r => r.ResolveStatuses.Any(
                    s => s.Status != StockIncreaseStatus.Accepted));
        }

        private static Result<List<(Product? Product, decimal Quantity)>> BuildOrderLine(
            IReadOnlyCollection<CreateProductOrder> items,
            IReadOnlyCollection<Product> productsInOrder)
        {
            var productsById = productsInOrder.ToDictionary(p => p.Id);
            List<(Product? Product, decimal Quantity)> lines = [];
            foreach (var item in items)
            {
                if (!productsById.TryGetValue(item.ProductId, out var product))
                    return Errors<Product>.ObjectNotExists(item.ProductId);

                lines.Add((product, item.Quantity));
            }

            return lines;
        }

        private async Task AssignItemsToOrder(
            IReadOnlyCollection<CreateProductOrder> items,
            Order registeredOrder,
            CancellationToken ct)
        {
            await assignOrderItems.Assing(
                [.. items.Select(p => p.Adapt<OrderItem>())],
                registeredOrder,
                ct);
        }

        private async Task<Customer> AssignCustomer(
            CustomerInformation customer,
            CancellationToken ct)
        {
            return await assignOrderUser.AssignAsync(
                customer.Adapt<Customer>(),
                ct);
        }

        private async Task<Order> RegisterOrder(
            OrderInformation information,
            Customer assignedCustomer,
            CancellationToken ct)
        {
            return await orderRegistration.RegisterAsync(
                information.Adapt<Order>(),
                assignedCustomer,
                ct);
        }
    }
}

