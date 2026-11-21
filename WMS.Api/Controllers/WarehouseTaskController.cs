using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.WarehouseTasks.Command.Complete;
using WMS.Application.Features.WarehouseTasks.Command.Reassign;
using WMS.Application.Features.WarehouseTasks.Command.Start;
using WMS.Application.Features.WarehouseTasks.Query;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class WarehouseTaskController(ISender sender) : ControllerBase
    {
        [HttpGet("my")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Roles.Warehouseman)]
        public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetMyTasksQuery(), cancellationToken);
            return result.ToOkOrProblem();
        }

        [HttpPost("{id:int}/start")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = Roles.Warehouseman)]
        public async Task<IActionResult> Start([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new StartTaskCommand(id), cancellationToken);
            return result.ToNoContentOrProblem();
        }

        [HttpPost("{id:int}/complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = Roles.Warehouseman)]
        public async Task<IActionResult> Complete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new CompleteTaskCommand(id), cancellationToken);
            return result.ToNoContentOrProblem();
        }

        [HttpPut("{id:int}/assignee/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Reassign([FromRoute] int id, [FromRoute] int userId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ReassignTaskCommand(id, userId), cancellationToken);

            return result.ToNoContentOrProblem();
        }
    }
}
