using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Update
{
    public class UpdateProductCommandHandler(
        IUnitOfWorks unitOfWorks)
        : ICommandHandler<UpdateProductCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(
            UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            if (await ExistsProductAsync(command.Id,cancellationToken))
            {
                return Errors<Product>.ObjectNotExists(command.Id); ;
            }

            var product = ConvertToProduct(command);
            
            await UpdateProductAsync(product, cancellationToken);

            return true;
        }

        private async Task<bool> ExistsProductAsync(int id, CancellationToken ct)
        {
            var existsProduct = await unitOfWorks.Product.GetByIdAsync(id, ct);
            return existsProduct is null;
        }
        private static Product ConvertToProduct(UpdateProductCommand request)
        {
            return request.Adapt<Product>();

        }
        private async Task UpdateProductAsync(Product product, CancellationToken ct)
        {
            var existsProduct = await unitOfWorks.Product.GetByIdAsync(product.Id, ct);
            product.Adapt(existsProduct);
            unitOfWorks.Product.Update(existsProduct!);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
