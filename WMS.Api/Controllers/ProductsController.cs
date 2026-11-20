using Mapster;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Contracts;
using WMS.Api.Extensions;
using WMS.Application.Features.Products.Command.Create;
using WMS.Application.Features.Products.Command.Delete;
using WMS.Application.Features.Products.Command.Update;
using WMS.Application.Features.Products.Query.Get;
using WMS.Application.Features.Products.Query.List;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> GetById([FromRoute] GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);
            if(result.IsSuccess)
            {
                Response.Headers.ETag = ETagHelper.ToEtag(result.Value.Version);
            }
            return result.ToOkOrProblem(p => p.Adapt<ProductResponse>());
            
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command,cancellationToken);

            return result.ToCreatedAtActionOrProblem(nameof(GetById),nameof(ProductsController));
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ListProductQuery(), cancellationToken);
            return result.ToOkOrProblem(p => p.Adapt<List<ProductResponse>>());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateProductRequest request,
            [FromHeader(Name = "If-Match")] string? ifMatch,
            CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Price,
                request.UnityQuantity,
                ETagHelper.Parse(ifMatch));

            var result = await sender.Send(command, cancellationToken);

            return result.ToNoContentOrProblem();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToNoContentOrProblem();
        }

    }
}

