using Mapster;
using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed class CreateStockCommandHandler(IUnitOfWork unitOfWorks)
        : ICommandHandler<CreateStockCommand, Result<int>>
    {
        public async ValueTask<Result<int>> Handle(
            CreateStockCommand command,
            CancellationToken cancellationToken)
        {
            var verifyResult = await ExistsDataAsync(command, cancellationToken);

            if(verifyResult is not null)
            {
                return verifyResult;
            }
            var stock = ConvertToStock(command);
            
            await AddToDatabaseAsync(stock, cancellationToken);

            return stock.Id;
        }
        private async Task<Error?> ExistsDataAsync(CreateStockCommand command, CancellationToken ct)
        {
            if (await StockExistsAsync(command.ProductId, command.LocationId, ct))
            {
                return Errors<Stock>.ObjectExists;
            }
            var result = await VerifyStockHelper.VerifyAsync(
                unitOfWorks,
                command.LocationId,
                command.ProductId,
                ct);

            return result;
        }
        private async Task<bool> StockExistsAsync(int productId, int locationId, CancellationToken ct)
        {
            return await unitOfWorks.Stock.StockExistsAsync(productId, locationId, ct);

        }
        private async Task AddToDatabaseAsync(Stock stock, CancellationToken ct)
        {
            await unitOfWorks.Stock.AddAsync(stock, ct);

            await unitOfWorks.SaveChangesAsync(ct);
        }

        private Stock ConvertToStock(CreateStockCommand command) =>
            command.Adapt<Stock>();
    }

}
