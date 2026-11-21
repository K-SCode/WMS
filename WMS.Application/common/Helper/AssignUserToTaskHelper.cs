using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.common.Helper
{
    public static class AssignUserToTaskHelper
    {
        public static Result<WarehouseTask> AssignTo(int userId, WarehouseTask task)
        {
            if (task.Status is not (WarehouseStatus.Pending or WarehouseStatus.Assigned))
                return Errors.CannotAssign;

            task.AssignedUserId = userId;
            task.AssignedAt = DateTimeOffset.UtcNow;
            task.Status = WarehouseStatus.Assigned;
            task.UpdatedAt = DateTimeOffset.UtcNow;
            return task;
        }

        public static Result<WarehouseTask> Complete(int userId, WarehouseTask task)
        {
            if (task.AssignedUserId != userId)
            {
                return Errors.NotAssignedToYou;
            }

            if (task.Status != WarehouseStatus.InProgress)
            {
                return Errors.InvalidStatus("Completed");
            }

            task.Status = WarehouseStatus.Completed;
            task.UpdatedAt = DateTimeOffset.UtcNow;

            return task;
        }

        public static Result<WarehouseTask> Start(int userId, WarehouseTask task)
        {
            if (task.AssignedUserId != userId)
                return Errors.NotAssignedToYou;
            if (task.Status != WarehouseStatus.Assigned)
                return Errors.InvalidStatus("started");

            task.Status = WarehouseStatus.InProgress;
            task.UpdatedAt = DateTimeOffset.UtcNow;
            return task;
                    }
    }
}
