using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.AssignRole
{
    public class AssignRoleCommandHandler(IIdentityService identityService) : ICommandHandler<AssignRoleCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(AssignRoleCommand command, CancellationToken cancellationToken)
        {
            return await identityService.AssignRole(command.Email, command.Role);
        }
    }
}
