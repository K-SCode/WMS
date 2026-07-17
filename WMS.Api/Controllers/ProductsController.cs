using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mediator;
using WMS.Application.Features.Products.Query.Get;
using WMS.Application.Features.Products.Dtos;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ISender mediator) : ControllerBase
    {
        [HttpGet("api/[controller]/[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var product = await mediator.Send(new GetProductQuery(id), cancellationToken);
            if (product is null)
                return NotFound();
            return Ok(product);
        }
    }
}
