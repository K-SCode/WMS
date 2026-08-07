using Mediator;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Features.Locations.Command.Create;
using WMS.Application.Features.Locations.Command.Delete;
using WMS.Application.Features.Locations.Command.Update;
using WMS.Application.Features.Locations.Query.Get;
using WMS.Application.Features.Locations.Query.List;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateLocationCommand request,
            CancellationToken cancellationToken)
        {
            var locationId = await sender.Send(request, cancellationToken);
            if (locationId == 0)
                return Conflict(new {Error = "Location already exists"});
            return Ok();
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteLocationCommand request,
            CancellationToken cancellationToken)
        {
            var isDeleted = await sender.Send(request, cancellationToken);
            if(!isDeleted)
            {
                return NotFound(new { Error = $"Location with ID: {request.Id} not exists" });
            }

            return NoContent();
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateLocationCommand command, CancellationToken cancellationToken)
        {
            var isUpdated = await sender.Send(command, cancellationToken);
            if (!isUpdated)
                return NotFound(new { Error = "Location cannot be updated" });

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var location = await sender.Send(new GetLocationQuery(id), cancellationToken);
            if (location is null)
                return NotFound(new { Error = $"Location with ID: {id} not found" });

            return Ok(location);
        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var location = await sender.Send(new ListLocationQuery(), cancellationToken);
            if (location is null)
                return NotFound(new { Error = "No locations found" });
            return Ok(location);
        }
    }
}
