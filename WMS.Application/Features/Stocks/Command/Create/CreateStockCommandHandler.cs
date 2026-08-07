using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed class CreateStockCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<CreateStockCommand, int>
    {
        public async ValueTask<int> Handle(
            CreateStockCommand command,
            CancellationToken cancellationToken)
        {
            var stock = command.Adapt<Stock>();

            await unitOfWorks.Stock.AddAsync(stock,cancellationToken);
            await unitOfWorks.SaveChangesAsync(cancellationToken);
            
            return stock.Id;
        }
    }

}
