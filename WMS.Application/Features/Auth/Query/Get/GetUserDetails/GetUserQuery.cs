using Mediator;
using WMS.Application.Features.Auth.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.Query.Get.GetUserDetails
{
    public sealed record GetUserQuery(int Id) : IQuery<Result<UserDetailsDto>>;
}
