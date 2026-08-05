using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Locations.Command.Delete
{
    public sealed class DeleteLocationCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<DeleteLocationCommand, bool>
    {
        public async ValueTask<bool> Handle(DeleteLocationCommand command, CancellationToken cancellationToken)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(command.Id,cancellationToken);

            if (location is null)
                return false;

            unitOfWorks.Location.Delete(location,cancellationToken);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
