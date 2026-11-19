using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Update
{
    public class UpdateProductCommandHandler(
        IUnitOfWork unitOfWorks)
        : ICommandHandler<UpdateProductCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(
            UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            var product = await unitOfWorks.Product.GetByIdAsync(command.Id, cancellationToken);
            if (!await ExistsProduct(product!)) 
            {
                return Errors<Product>.ObjectNotExists(command.Id); ;
            }

            command.Adapt(product);
            unitOfWorks.Product.SetExpectedVersion(product!, command.Version);
            
            await UpdateProductAsync(product!, cancellationToken);

            return true;
        }

        private async Task<bool> ExistsProduct(Product product)
        {
            return product is not null;
        }
        private async Task UpdateProductAsync(Product product, CancellationToken ct)
        {
            unitOfWorks.Product.Update(product!);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
