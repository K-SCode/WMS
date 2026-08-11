using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed class UpdateLocationCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<UpdateLocationCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            if (await LocationExistsAsync(command,cancellationToken))
                return Errors<Location>.ObjectNotExists(command.Id);

            var location = await AdaptToLocationAsync(command, cancellationToken);

            await UpdateLocationAsync(location, cancellationToken);

            return true;
        }

        private async Task<bool> LocationExistsAsync(UpdateLocationCommand command, CancellationToken ct)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(command.Id, ct);
            var exists = await unitOfWorks.Location.LocationExistsAsync(
                command.Sector,
                command.Rack,
                command.Number,
                ct);
            return location is null && exists;
        }
        private async Task<Location> AdaptToLocationAsync(
            UpdateLocationCommand command,
            CancellationToken ct)
        {
            var location = await GetLocationAsync(command.Id, ct);
            return command.Adapt(location);
        }
        private async Task<Location> GetLocationAsync(int id, CancellationToken ct)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(id, ct);
            return location!;
        }
        
        private async Task UpdateLocationAsync(Location location, CancellationToken ct)
        {
            unitOfWorks.Location.Update(location);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
