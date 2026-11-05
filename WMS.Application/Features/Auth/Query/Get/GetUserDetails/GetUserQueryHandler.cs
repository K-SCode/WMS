using Dapper;
using Mediator;
using WMS.Application.Features.Auth.Dtos;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.Query.Get.GetUserDetails
{
    public class GetUserQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<GetUserQuery, Result<UserDetailsDto>>
    {
        private const string sql = """
            SELECT
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber AS Phone,
                r.Name AS Role,
                u.CratedAt,
                u.IsActive
            FROM [AspNetUsers] u
            LEFT JOIN [AspNetUserRoles] ur ON ur.UserId = u.Id
            LEFT JOIN [AspNetRoles] r ON r.Id = ur.RoleId
            WHERE u.Id = @Id
            """;
        public async ValueTask<Result<UserDetailsDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(sql, new {query.Id}, cancellationToken: cancellationToken);
            using var connection = connectionFactory.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<UserDetailsDto>(command);
            if(result is null)
            {
                return Errors<GetUserQuery>.ObjectNotExists(query.Id);
            }
            return result;
        }
    }
}
