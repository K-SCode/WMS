using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Command.Create
{
    public sealed class CreateProductCommandHandler(IUnitOfWorks unitOfWorks) : IRequestHandler<CreateProductCommand, int>
    {
        public async ValueTask<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();
            await unitOfWorks.Product.AddAsync(product, cancellationToken);
            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
