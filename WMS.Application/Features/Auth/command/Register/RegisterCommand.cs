using Mediator;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;

namespace WMS.Application.Features.Auth.command.Register
{
    public sealed record RegisterCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone) : ICommand<Result<bool>>;
}
