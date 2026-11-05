using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.Auth.command.AssignRole;
using WMS.Application.Features.Auth.Query.Get.GetUserDetails;
using WMS.Application.Features.Auth.Query.List.ListUsers;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserController(ISender sender) : Controller
    {
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ListUsersQuery(), cancellationToken);
            return result.ToOkOrProblem();
        }
        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] GetUserQuery query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return result.ToOkOrProblem();
        }
    }
}
