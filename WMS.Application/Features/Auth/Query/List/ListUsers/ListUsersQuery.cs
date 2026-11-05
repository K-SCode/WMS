using Mediator;
using WMS.Application.Features.Auth.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.Query.List.ListUsers
{
    public sealed record ListUsersQuery : IQuery<Result<IReadOnlyCollection<UserDto>>>;
}
