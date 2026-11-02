using Mediator;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;

namespace WMS.Application.Features.Auth.command.Login
{
    public class LoginCommandHandler(IIdentityService identityService) : ICommandHandler<LoginCommand, Result<AuthToken>>
    {
        public async ValueTask<Result<AuthToken>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            return await identityService.LoginUserAsync(command.Email, command.Password, cancellationToken);
        }
    }
}
