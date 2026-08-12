using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
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
            var result = await sender.Send(request, cancellationToken);

            return result.ToCreatedAtActionOrProblem(nameof(GetById),nameof(StocksController));
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteStockCommand request,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);

            return result.ToNoContentOrProblem();
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateStockCommand command,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContentOrProblem();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetStockQuery(id), cancellationToken);
            return result.ToOkOrProblem();
        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ListStockQuery(), cancellationToken);

            return result.ToOkOrProblem();
        }
    }
}
