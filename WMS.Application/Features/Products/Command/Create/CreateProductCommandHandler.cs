using Mapster;
using Mediator;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandHandler(
        IUnitOfWork unitOfWorks)
        : ICommandHandler<CreateProductCommand, Result<int>>
    {
        public async ValueTask<Result<int>> Handle(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            if(await ExistsByBarcodeAsync(command.Barcode, cancellationToken))
            {
                return Errors<Product>.ObjectExists;
            }

            var product = ConvertToProduct(command);

            await CreateProductAsync(product,cancellationToken);
            return product.Id;
        }

        private async Task<bool> ExistsByBarcodeAsync(
            string barcode,
            CancellationToken ct)
        {
            var result =
                await unitOfWorks.Product.GetByBarcodeAsync(barcode, ct);
            return result is not null; 
        }
        private async Task CreateProductAsync(Product product, CancellationToken ct)
        {
            await unitOfWorks.Product.AddAsync(product, ct);
            await unitOfWorks.SaveChangesAsync(ct);
        }
        private static Product ConvertToProduct(CreateProductCommand request)
        {
            return request.Adapt<Product>();

        }
    }
}
