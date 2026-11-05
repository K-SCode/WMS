using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.Auth.command.Login;
using WMS.Application.Features.Auth.command.Register;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

            return result.ToOkOrProblem();
        }

        
    }
}
