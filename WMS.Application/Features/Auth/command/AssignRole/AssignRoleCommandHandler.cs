using Mediator;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.AssignRole
{
    public class AssignRoleCommandHandler(IIdentityService identityService) : ICommandHandler<AssignRoleCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(AssignRoleCommand command, CancellationToken cancellationToken)
        {
            if(!Roles.All.Contains(command.Role))
            {
                return Errors.RoleNotExists;
            }
            return await identityService.AssignRole(command.Email, command.Role);
        }
    }
}
