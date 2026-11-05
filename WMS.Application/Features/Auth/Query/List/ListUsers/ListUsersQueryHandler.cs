using Dapper;
using Mediator;
using WMS.Application.Features.Auth.Dtos;
using WMS.Application.Interfaces;
using WMS.Core.Common;

namespace WMS.Application.Features.Auth.Query.List.ListUsers
{
    public class ListUsersQueryHandler(ISqlConnectionFactory connectionFactory) : IQueryHandler<ListUsersQuery, Result<IReadOnlyCollection<UserDto>>>
    {
        private const string sql = """
            SELECT
                u.Id,
                u.Email,
                u.PhoneNumber AS Phone,
                r.Name AS Role,
                u.FirstName,
                u.LastName
            FROM [AspNetUsers] u
            LEFT JOIN [AspNetUserRoles] ur ON ur.UserId = u.Id
            LEFT JOIN [AspNetRoles] r ON r.Id = ur.RoleId
            """;
        public async ValueTask<Result<IReadOnlyCollection<UserDto>>> Handle(
            ListUsersQuery query,
            CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(sql, cancellationToken);

            using var connection = connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<UserDto>(command);
            return result.AsList();
        }
    }
}
