using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public sealed class UpdateStockCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<UpdateStockCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(UpdateStockCommand command, CancellationToken cancellationToken)
        {
            if (await StockExists(command.Id, cancellationToken))
                return Errors<Stock>.ObjectNotExists(command.Id);

            var stock = await ConvertToStock(command, cancellationToken);

            await UpdateStock(stock, cancellationToken);


            return true;
        }

        private async Task<bool> StockExists(int id,CancellationToken ct)
        {
            var stock = await  unitOfWorks.Stock.GetByIdAsync(id, ct);
            return stock is null;
        }

        private async Task<Stock> ConvertToStock(UpdateStockCommand command,CancellationToken ct)
        {
            var stock = await unitOfWorks.Stock.GetByIdAsync(command.Id, ct);
            return command.Adapt(stock)!;
        }
        private async Task UpdateStock(Stock stock,CancellationToken ct)
        {
            unitOfWorks.Stock.Update(stock);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
