using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.AssignRole
{
    public sealed record AssignRoleCommand(string Email, string Role) : ICommand<Result<bool>>;
}
