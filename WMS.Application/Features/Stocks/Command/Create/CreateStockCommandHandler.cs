using Mapster;
using Mediator;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed class CreateStockCommandHandler(IUnitOfWorks unitOfWorks)
        : ICommandHandler<CreateStockCommand, Result<int>>
    {
        public async ValueTask<Result<int>> Handle(
            CreateStockCommand command,
            CancellationToken cancellationToken)
        {
            if(await StockExistsAsync(command,cancellationToken))
            {
                return Errors<Stock>.ObjectExists;
            }
            var stock = ConvertToStock(command);
            
            await AddToDatabaseAsync(stock, cancellationToken);

            return stock.Id;
        }

        private async Task<bool> StockExistsAsync(CreateStockCommand command,CancellationToken ct)
        {
           return await unitOfWorks.Stock.StockExistsAsync(command.ProductId, command.LocationId, ct);
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
