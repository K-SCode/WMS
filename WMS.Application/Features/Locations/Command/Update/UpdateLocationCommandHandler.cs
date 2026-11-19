using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Locations.Command.Update
{
    public sealed class UpdateLocationCommandHandler(IUnitOfWork unitOfWorks) : ICommandHandler<UpdateLocationCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var location = await unitOfWorks.Location.GetByIdAsync(command.Id, cancellationToken);
            if (location is null)
                return Errors<Location>.ObjectNotExists(command.Id);

            if (await IsTakenByAnotherLocation(location, command, cancellationToken))
            {
                return Errors<Location>.ObjectExists;
            }
            command.Adapt(location);
            unitOfWorks.Location.SetExpectedVersion(location, command.Version);

            await UpdateLocationAsync(location, cancellationToken);

            return true;
        }

        private async Task<bool> IsTakenByAnotherLocation(Location location, UpdateLocationCommand command, CancellationToken ct)
        {
            bool addressChanged =
                location.Sector != command.Sector ||
                location.Rack != command.Rack ||
                location.Number != command.Number;
            return addressChanged &&
                await unitOfWorks.Location.LocationExistsAsync(
                    command.Sector,
                    command.Rack,
                    command.Number,
                    ct);
        }
        private async Task UpdateLocationAsync(Location location, CancellationToken ct)
        {
            unitOfWorks.Location.Update(location);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
