using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Features.Stocks.Command.Create;
using WMS.Application.Features.Stocks.Command.Delete;
using WMS.Application.Features.Stocks.Command.Update;
using WMS.Application.Features.Stocks.Query.Get;
using WMS.Application.Features.Stocks.Query.List;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
           [FromBody] CreateStockCommand request,
           CancellationToken cancellationToken)
        {
            var stockId = await sender.Send(request, cancellationToken);
            if (stockId == 0)
                return Conflict(new { Error = "Stock already exists" });
            return CreatedAtAction(nameof(GetById),new { Id = stockId },stockId);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteStockCommand request,
            CancellationToken cancellationToken)
        {
            var isDeleted = await sender.Send(request, cancellationToken);
            if (!isDeleted)
            {
                return NotFound(new { Error = $"Stock with ID: {request.Id} not exists" });
            }

            return NoContent();
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateStockCommand command,
            CancellationToken cancellationToken)
        {
            var isUpdated = await sender.Send(command, cancellationToken);
            if (!isUpdated)
                return NotFound(new { Error = "Stock cannot be updated" });

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var stock = await sender.Send(new GetStockQuery(id), cancellationToken);
            if (stock is null)
                return NotFound(new { Error = $"Stock with ID: {id} not found" });

            return Ok(stock);
        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var stocks = await sender.Send(new ListStockQuery(), cancellationToken);
            if (stocks is null)
                return NotFound(new { Error = "No stocks found" });
            return Ok(stocks);
        }
    }
}
