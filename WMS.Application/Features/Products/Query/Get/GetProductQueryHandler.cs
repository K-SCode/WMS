using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Interfaces;

namespace WMS.Application.Features.Products.Query.Get
{
    public sealed class GetProductQueryHandler(
        IAppDbContext context) : IRequestHandler<GetProductQuery, ProductDto?>
    {
        public async ValueTask<ProductDto?> Handle(
            GetProductQuery request,
            CancellationToken cancellationToken)
        {
            ProductDto? result = await context.Products
                .AsNoTracking()
                .ProjectToType<ProductDto>()
                .FirstOrDefaultAsync(p => p.Id == request.Id,cancellationToken);
            return result;
        }
    }
}
