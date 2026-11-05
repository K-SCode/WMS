using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.command.Register
{
    public sealed record RegisterCommand(
        string Email,
        string Password,
        string RepeatPassword,
        string FirstName,
        string LastName,
        string Phone) : ICommand<Result<bool>>;
}
