using Mediator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Delete
{
    public sealed class DeleteStockCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<DeleteStockCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(
            DeleteStockCommand command,
            CancellationToken cancellationToken)
        {

            if (await StockExistsAsync(command.Id, cancellationToken))
                return Errors<Stock>.ObjectNotExists(command.Id);

            await DeleteFromDataBase(command.Id, cancellationToken);

            return true;
        }

        private async Task<bool> StockExistsAsync(int id,CancellationToken ct)
        {
            var stock = await unitOfWorks.Stock.GetByIdAsync(id,ct);
            return stock is null;
        }
        private async Task DeleteFromDataBase(int id, CancellationToken ct)
        {
            var stock = await unitOfWorks.Stock.GetByIdAsync(id, ct);
            unitOfWorks.Stock.Delete(stock!, ct);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
