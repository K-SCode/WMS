using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public sealed class UpdateStockCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<UpdateStockCommand, bool>
    {
        public async ValueTask<bool> Handle(UpdateStockCommand command, CancellationToken cancellationToken)
        {
            var existsStock = await unitOfWorks.Stock.GetByIdAsync(command.Id, cancellationToken);

            if (existsStock is null)
                return false;

            command.Adapt(existsStock);
            unitOfWorks.Stock.Update(existsStock);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
