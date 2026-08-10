using Microsoft.AspNetCore.Mvc;
using Mediator;
using WMS.Application.Features.Products.Query.Get;
using WMS.Application.Features.Products.Command.Create;
using WMS.Application.Features.Products.Query.List;
using WMS.Application.Features.Products.Command.Update;
using WMS.Application.Features.Products.Command.Delete;
using WMS.Api.Extensions;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);

            return result.ToOkOrProblem();
            
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command,cancellationToken);

            return result.ToCreatedAtActionOrProblem(nameof(GetById),nameof(ProductsController));
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ListProductQuery(), cancellationToken);
            return result.ToOkOrProblem();
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);


            return result.ToNoContentOrProblem();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToNoContentOrProblem();
        }

    }
}
