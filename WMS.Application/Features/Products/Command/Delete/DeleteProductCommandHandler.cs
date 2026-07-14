using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Delete
{
    public class DeleteProductCommandHandler(
        IUnitOfWorks unitOfWorks) : IRequestHandler<DeleteProductCommand, bool>
    {
        public async ValueTask<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWorks.Product.GetByIdAsync(request.Id, cancellationToken);
            if (product is null)
                return false;

            unitOfWorks.Product.Delete(product,cancellationToken);

            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
