using Mediator;
using WMS.Application.Features.Products.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Query.List
{
    public sealed record ListProductQuery : IQuery<Result<IEnumerable<ProductDto>>>;
}
