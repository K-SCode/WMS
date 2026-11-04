using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Query.Get
{
    public sealed record GetProductQuery(int Id) : IQuery<Result<ProductDto>>;
}
