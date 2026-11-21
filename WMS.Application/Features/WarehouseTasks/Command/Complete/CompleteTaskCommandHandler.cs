using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.WarehouseTasks.Command.Complete
{
    public class CompleteTaskCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser) : ICommandHandler<CompleteTaskCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await unitOfWork.Warehouse.GetByIdAsync(command.Id, cancellationToken);
            if (task is null)
            {
                return Errors<WarehouseTask>.ObjectNotExists(command.Id);
            }

            var changedStatus = AssignUserToTaskHelper.Complete(currentUser.UserId,task);

            if (!changedStatus.IsSuccess)
            {
                return changedStatus.Error!;
            }
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return changedStatus.IsSuccess;
        }
    }
}
