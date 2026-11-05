using Mapster;
using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public sealed class UpdateStockCommandHandler(IUnitOfWork unitOfWorks) : ICommandHandler<UpdateStockCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(UpdateStockCommand command, CancellationToken cancellationToken)
        {
            var verifyResult = await ExistsDataAsync(command, cancellationToken);
            if (verifyResult is not null)
            {
                return verifyResult;
            }

            var stock = await ConvertToStock(command, cancellationToken);

            await UpdateStock(stock, cancellationToken);

            return true;
        }

        private async Task<Error?> ExistsDataAsync(UpdateStockCommand command, CancellationToken ct)
        {
            if (await StockExistsAsync(command.Id, ct))
            {
                return Errors<Stock>.ObjectNotExists(command.Id);
            }
            var result = await VerifyStockHelper.VerifyAsync(
                unitOfWorks,
                command.LocationId,
                command.ProductId,
                ct);

            return result;
        }

        private async Task<bool> StockExistsAsync(int id,CancellationToken ct)
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
