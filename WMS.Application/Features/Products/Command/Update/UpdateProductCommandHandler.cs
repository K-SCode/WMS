using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Command.Delete;
using WMS.Application.Interfaces;
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
            var existsProduct = await unitOfWorks.Product.GetByIdAsync(request.Id, cancellationToken);
            if (existsProduct is null)
                return false;
            request.Adapt(existsProduct);
            unitOfWorks.Product.Update(existsProduct);
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
