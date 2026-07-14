using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using Mapster;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Update
{
    public class UpdateProductCommandHandler(
        IUnitOfWorks unitOfWorks) 
        : IRequestHandler<UpdateProductCommand, bool>
    {
        public async ValueTask<bool> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await unitOfWorks.Product.GetByIdAsync(request.Id, cancellationToken);
            if (product is null)
                return false;

            unitOfWorks.Product.Update(product);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
