using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.WarehouseTasks.Command.Reassign
{
    public class ReassignTaskCommandHandler(
        IUnitOfWork unitOfWork,
        IWorkerDirectory workerDirectory)
        : ICommandHandler<ReassignTaskCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(ReassignTaskCommand command, CancellationToken cancellationToken)
        {
            WarehouseTask? task = await unitOfWork.Warehouse.GetByIdAsync(command.Id, cancellationToken);
            if(task is null)
                return Errors<WarehouseTask>.ObjectNotExists(command.Id);

            var workers = await workerDirectory.GetActiveWarehousemenIdsAsync(cancellationToken);

            if(!workers.Contains(command.UserId))
                return Errors.WorkerNotAvailable(command.UserId);

            var result = AssignUserToTaskHelper.AssignTo(command.UserId, task);
            if(!result.IsSuccess)
            {
                return result.Error!;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
