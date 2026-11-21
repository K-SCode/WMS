using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.WarehouseTasks.Command.Start
{
    public class StartTaskCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
        : ICommandHandler<StartTaskCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(StartTaskCommand command, CancellationToken cancellationToken)
        {
            WarehouseTask? task = await unitOfWork.Warehouse.GetByIdAsync(command.Id, cancellationToken);
            if(task is null)
                return Errors<WarehouseTask>.ObjectNotExists(command.Id);

            var result = AssignUserToTaskHelper.Start(currentUser.UserId,task);
            if (!result.IsSuccess)
                return result.Error!;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return result.IsSuccess;
        }
    }
}
