using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mediator;
using WMS.Application.Features.Products.Query.Get;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Features.Products.Command.Create;
using WMS.Application.Features.Products.Query.List;
using WMS.Application.Features.Products.Command.Update;
using WMS.Application.Features.Products.Command.Delete;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ISender mediator) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var product = await mediator.Send(new GetProductQuery(id), cancellationToken);
            if (product is null)
                return NotFound(new {Error = $"Product with ID: {id} not found"});

            return Ok(product);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await mediator.Send(command,cancellationToken);
            if (productId == 0)
                return Conflict(new {Error = "Product already exists"});

            return CreatedAtAction(nameof(GetById),new { id = productId }, productId);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await mediator.Send(new ListProductQuery(), cancellationToken);
            if (products is null)
                return NotFound(new {Error = "No products found"});
            return Ok(products);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var isUpdated = await mediator.Send(command, cancellationToken);
            if (!isUpdated)
                return NotFound(new { Error = "Product cannot be updated"});

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var isDeleted = await mediator.Send(command, cancellationToken);
            if (!isDeleted)
                return NotFound(new { Error = "Product not exists"});

            return NoContent();
        }

    }
}
