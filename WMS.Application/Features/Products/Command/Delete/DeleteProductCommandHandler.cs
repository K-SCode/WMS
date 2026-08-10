using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Delete
{
    public class DeleteProductCommandHandler(
        IUnitOfWorks unitOfWorks) : ICommandHandler<DeleteProductCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            if(await ExistsProductAsync(command.Id,cancellationToken))
            {
                return Errors<Product>.ObjectNotExists(command.Id);
            }

            await DeleteProductAsync(command.Id, cancellationToken);

            return true;
        }
        private async Task<bool> ExistsProductAsync(int id, CancellationToken ct)
        {
            var product = await unitOfWorks.Product.GetByIdAsync(id, ct);
            return product is null;
        }

        private async Task DeleteProductAsync(int id , CancellationToken ct)
        {
            var product = await unitOfWorks.Product.GetByIdAsync(id, ct);
            unitOfWorks.Product.Delete(product!, ct);
            await unitOfWorks.SaveChangesAsync(ct);
        }
    }
}
