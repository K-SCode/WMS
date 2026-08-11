using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Command.Create
{
    public sealed class CreateLocationCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<CreateLocationCommand, Result<int>>
    {
        public async ValueTask<Result<int>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
        {
            if(await LocationExistsAsync(command,cancellationToken))
            {
                return Errors<Location>.ObjectExists;
            }

            var location = ConvertToLocation(command);

            await AddLocationAsync(location, cancellationToken);

            return location.Id;
        }
        
        private async Task<bool> LocationExistsAsync(CreateLocationCommand command, CancellationToken ct) =>
            await unitOfWorks.Location.LocationExistsAsync(command.Sector, command.Rack, command.Number, ct);

        private Location ConvertToLocation(CreateLocationCommand command) =>
            command.Adapt<Location>();
        private async Task AddLocationAsync(Location location, CancellationToken ct)
        {
            await unitOfWorks.Location.AddAsync(location, ct);
            await unitOfWorks.SaveChangesAsync(ct);

        }
    }
}
