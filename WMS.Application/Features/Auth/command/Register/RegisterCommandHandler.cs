using Mediator;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.Register
{
    public class RegisterCommandHandler(
        IIdentityService identityService) : ICommandHandler<RegisterCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            return await identityService.RegisterUserAsync(
                command.FirstName,
                command.LastName,
                command.Password,
                command.Email,
                command.Phone,
                cancellationToken);
        }
    }
}
