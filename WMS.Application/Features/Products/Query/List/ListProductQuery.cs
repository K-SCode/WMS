using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Products.Dtos;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed record ListProductQuery : IRequest<List<ProductDto>>;
}
