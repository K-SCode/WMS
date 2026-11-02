using Mediator;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.Auth.command.Login;
using WMS.Application.Features.Auth.command.Register;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

            return result.ToOkOrProblem();
        }
    }
}
