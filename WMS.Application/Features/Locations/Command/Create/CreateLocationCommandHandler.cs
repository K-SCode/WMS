using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Command.Create
{
    public sealed class CreateLocationCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<CreateLocationCommand, int>
    {
        public async ValueTask<int> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
        {
            var location = command.Adapt<Location>();

            await unitOfWorks.Location.AddAsync(location, cancellationToken);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return location.Id;
        }
    }
}
