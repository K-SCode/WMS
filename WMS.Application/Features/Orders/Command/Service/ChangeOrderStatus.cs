using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public static class ChangeOrderStatus
    {
        public static void ChangeStatus(Order order, OrderStatus status)
        {
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = status;
        }
    }
}
