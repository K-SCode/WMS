using Mapster;
using Mediator;
using WMS.Application.Features.Auth.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.Login
{
    public class LoginCommandHandler(IIdentityService identityService) : ICommandHandler<LoginCommand, Result<LoginDto>>
    {
        public async ValueTask<Result<LoginDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await identityService.LoginUserAsync(command.Email, command.Password, cancellationToken);
            return result.Value.Adapt<LoginDto>();
        }
    }
}
