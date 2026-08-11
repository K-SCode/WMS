using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Command.Delete
{
    public sealed class DeleteLocationCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<DeleteLocationCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(DeleteLocationCommand command, CancellationToken cancellationToken)
        {

            if (await LocationExistsAsync(command.Id, cancellationToken))
                return Errors<Location>.ObjectNotExists(command.Id);

            try
            {
                await DeleteLocationAsync(command.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                return Errors<Location>.ObjectCanNotBeDelete;
            }

            return true;
        }
        private async Task<bool> LocationExistsAsync(int id, CancellationToken ct)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(id, ct);
            return location is null;
        }
        private async Task DeleteLocationAsync(int id, CancellationToken ct)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(id, ct);
            unitOfWorks.Location.Delete(location!, ct);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
