using Shouldly;
using WMS.Application.common.Exceptions;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Repositories;
using Xunit;

namespace WMS.Tests.IntegrationTests
{
    public class ConcurrencyIntegrationTests(SqlServerDatabaseFixture database)
        :IClassFixture<SqlServerDatabaseFixture>
    {
        [Fact]
        public async Task TwoUsersChangesSameStock_SecondSaveFails_FirstChangesIsNotOverwritten()
        {
            var ct = TestContext.Current.CancellationToken;
            int stockId = await CreateStockAsync(20, ct);

            await using ApplicationDbContext userAContext = database.CreateContext();
            await using ApplicationDbContext userBContext = database.CreateContext();

            var userA = new UnitOfWorks(userAContext);
            var userB = new UnitOfWorks(userBContext);

            Stock stockA = await userA.Stock.GetByIdAsync(stockId, ct);
            Stock stockB = await userB.Stock.GetByIdAsync(stockId, ct);

            stockA.Quantity -= 8;
            await userA.SaveChangesAsync(ct);

            stockB.Quantity -= 3;
            await Should.ThrowAsync<ConcurrencyConflictException>(() => userB.SaveChangesAsync(ct));

            await using ApplicationDbContext check = database.CreateContext();
            Stock saved = await check.Stocks.FindAsync([stockId], ct);
            saved.Quantity.ShouldBe(12);
        }

        private async Task<int> CreateStockAsync(int quantity, CancellationToken ct)
        {
            await using ApplicationDbContext context = database.CreateContext();

            var stock = new Stock
            {
                Product = new Product
                {
                    Name = "Test product",
                    Price = 20m,
                    Barcode = "1231231231231",
                    UnitQuantity = "szt"
                },
                Localization = new Location
                {
                    Sector = 'T',
                    Rack = 1,
                    Number = 1
                },
                Quantity = quantity,
                MaxQuantity = 100
            };
            context.Stocks.Add(stock);
            await context.SaveChangesAsync(ct);
            return stock.Id;
        }
    }
}
