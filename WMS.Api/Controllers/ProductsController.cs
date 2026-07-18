using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mediator;
using WMS.Application.Features.Products.Query.Get;
using WMS.Application.Features.Products.Dtos;
using WMS.Application.Features.Products.Command.Create;
using WMS.Application.Features.Products.Query.List;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ISender mediator) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var product = await mediator.Send(new GetProductQuery(id), cancellationToken);
            if (product is null)
                return NotFound();
            return Ok(product);
        }
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await mediator.Send(command,cancellationToken);
            if (productId == 0)
                return BadRequest();

            return Created($"/api/products/{productId}", new {id = productId});
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetAll(CancellationToken cancellationToken)
        {
            var products = await mediator.Send(new ListProductQuery(), cancellationToken);
            if (products is null)
                return NotFound();
            return Ok(products);
        }

    }
}
