using WMS.Application.Features.Orders.Dto;
using WMS.Core.Common;

namespace WMS.Application.Interfaces.Orders
{
    public interface ICreateOrderProcess
    {
        Task<Result<int>> ProcessAsync(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> items,
            OrderInformation information,
            CancellationToken ct);
    }
}
