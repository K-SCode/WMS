using Microsoft.EntityFrameworkCore;
using WMS.Infrastructure.Data;
using Xunit;

namespace WMS.Tests.IntegrationTests
{
    public sealed class SqlServerDatabaseFixture : IAsyncLifetime
    {
        private const string ConnectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=WMS_IntegrationTests;Integrated Security=true;TrustServerCertificate=true";

        public ApplicationDbContext CreateContext() =>
            new(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public async ValueTask InitializeAsync()
        {
            await using var context = CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
        public ValueTask DisposeAsync() =>
            ValueTask.CompletedTask;
        
    }
}
