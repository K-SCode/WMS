using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandHandler(
        IUnitOfWorks unitOfWorks)
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
            var product =
                await unitOfWorks.Product.GetByBarcodeAsync(barcode, ct);
            return product is not null; 
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
