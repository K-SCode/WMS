using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.Auth.command.AssignRole;
using WMS.Application.Features.Auth.command.Login;
using WMS.Application.Features.Auth.command.Register;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ISender sender) : ControllerBase
    {
        [HttpPost("[action]")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

            return result.ToOkOrProblem();
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }
    }
}
