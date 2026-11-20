using Mapster;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Contracts;
using WMS.Api.Extensions;
using WMS.Application.Features.Locations.Command.Create;
using WMS.Application.Features.Locations.Command.Delete;
using WMS.Application.Features.Locations.Command.Update;
using WMS.Application.Features.Locations.Query.Get;
using WMS.Application.Features.Locations.Query.List;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class LocationController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Create(
            [FromBody] CreateLocationCommand request,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);
            return result.ToOkOrProblem();
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteLocationCommand request,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);
            return result.ToNoContentOrProblem();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateLocationRequest request,
            [FromHeader(Name = "If-Match")] string? ifMatch,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLocationCommand(
                id,
                request.Sector,
                request.Rack,
                request.Number,
                ETagHelper.Parse(ifMatch));

            var result = await sender.Send(command, cancellationToken);

            return result.ToNoContentOrProblem();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetLocationQuery(id), cancellationToken);
            if(result.IsSuccess)
            {
                Response.Headers.ETag =
                    ETagHelper.ToEtag(result.Value.Version);
            }

            return result.ToOkOrProblem(l => l.Adapt<LocationResponse>());
        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ListLocationQuery(), cancellationToken);
            return result.ToOkOrProblem(l => l.Adapt<List<LocationResponse>>());
        }
    }
}
