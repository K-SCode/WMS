using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Stocks.Command.Delete
{
    public sealed class DeleteStockCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<DeleteStockCommand, bool>
    {
        public async ValueTask<bool> Handle(
            DeleteStockCommand command,
            CancellationToken cancellationToken)
        {
            var stock = await unitOfWorks.Stock.GetByIdAsync(command.Id,cancellationToken);
            
            if(stock is null)
                return false;
            unitOfWorks.Stock.Delete(stock,cancellationToken);
            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
