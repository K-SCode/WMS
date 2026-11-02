using Mediator;
using WMS.Core.Common;
using WMS.Core.Entities.Auth;

namespace WMS.Application.Features.Auth.command.Login
{
    public sealed record LoginCommand(string Email, string Password) : ICommand<Result<AuthToken>>;
}
