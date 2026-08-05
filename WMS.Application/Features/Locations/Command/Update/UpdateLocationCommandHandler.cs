using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed class UpdateLocationCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<UpdateLocationCommand, bool>
    {
        public async ValueTask<bool> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var existsLocation = await unitOfWorks.Location.GetByIdAsync(command.Id, cancellationToken);
            if (existsLocation is null)
                return false;

            command.Adapt(existsLocation);
            unitOfWorks.Location.Update(existsLocation);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
