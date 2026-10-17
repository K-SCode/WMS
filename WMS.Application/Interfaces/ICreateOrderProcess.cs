using WMS.Application.Features.Orders.Dto;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICreateOrderProcess
    {
        Task<int> ProcessAsync(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> items,
            OrderInformation information,
            CancellationToken ct);
    }
}
