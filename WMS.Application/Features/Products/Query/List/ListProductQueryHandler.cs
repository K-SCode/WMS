using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Dtos;
using Mapster;
using MapsterMapper;
using WMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed class ListProductQueryHandler(
        IAppDbContext context) : IRequestHandler<ListProductQuery, List<ProductDto>>
    {
        public async ValueTask<List<ProductDto>> Handle(
            ListProductQuery request,
            CancellationToken cancellationToken)
        {
            var result = await context.Products
                .AsNoTracking()
                .ProjectToType<ProductDto>()
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}
