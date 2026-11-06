using Moq;
using Shouldly;
using WMS.Application.common.Service;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.Services
{
    public class DecreaseQuantityTests
    {
        private readonly Mock<IUnitOfWork> _moq;
        private readonly DecreaseQuantity _sut;

        private const string Barcode = "1231231231231";

        public DecreaseQuantityTests() 
        {
            _moq = new Mock<IUnitOfWork>();
            _sut = new DecreaseQuantity(_moq.Object);
        }
        private void SetupStock(params Stock[] stocks)
        {
            var map = new Dictionary<string, IReadOnlyCollection<Stock>>
            {
                [Barcode] = stocks
            };

            _moq.Setup(
               u => u.Stock.GetStocksWithProductByBarcodeAsync(
                   It.IsAny<IReadOnlyCollection<string>>(),
                   It.IsAny<CancellationToken>())
               ).ReturnsAsync(map);
        }

        private void SetupProduct(bool exists)
        {
            var map = new Dictionary<string, bool> { [Barcode] = exists };
            _moq.Setup(
               u => u.Product.ExistsByBarcodes(
                   It.IsAny<IReadOnlyCollection<string>>(),
                   It.IsAny<CancellationToken>())
               ).ReturnsAsync(map);
        }

        private static Stock CreateStock(int locationId, int quantity, int productId)
        {
            return new Stock
            {
                LocationId = locationId,
                MaxQuantity = 100,
                Quantity = quantity,
                ProductId = productId
            };
        }

        private async Task<IReadOnlyList<StockResult>> DecreaseAsync(decimal quantity)
        {
            return await _sut.DecreaseAsync(
                [new Model(Barcode, quantity)],
                p => p.Barcode,
                p => p.Quantity,
                CancellationToken.None);
        }

        private void VerifyMoq()
        {
            _moq.Verify(
                u => u.Product.ExistsByBarcodes(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _moq.Verify(
                u => u.Stock.GetStocksWithProductByBarcodeAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _moq.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
        private sealed record Model(string Barcode, decimal Quantity);
        [Fact]
        public async Task DecreaseAsync_QuantitnyIsGreaterThanOneStockQuantity_DecreaseFromFewStocks()
        {
            //Arrange
            var stock1 = CreateStock(1, 10, 1);
            var stock2 = CreateStock(1, 50, 2);
            SetupStock(stock1, stock2);
            SetupProduct(true);

            //Act

            IReadOnlyList<StockResult> result = await DecreaseAsync(30m);

            //Assert
            var row = result.ShouldHaveSingleItem().ResolveStatuses.ToArray();
            row.Length.ShouldBe(2);

            row[0].Quantity.ShouldBe(10m);
            row[0].Status.ShouldBe(StockManageStatus.Accepted);
            stock1.Quantity.ShouldBe(0m);

            row[1].Quantity.ShouldBe(20m);
            row[1].Status.ShouldBe(StockManageStatus.Accepted);
            stock2.Quantity.ShouldBe(30m);

            VerifyMoq();
        }
        [Fact]
        public async Task DecreaseAsync_NotEnoughStockQuantity_AllRowsHaveNotEnoughQuantityStatus()
        {
            //arrange
            var stock1 = CreateStock(1, 20, 1);
            var stock2 = CreateStock(2, 20, 1);
            SetupStock(stock1,stock2);
            SetupProduct(true);

            //Act
            var result = await DecreaseAsync(50);
            //Assert
            var row = result.ShouldHaveSingleItem().ResolveStatuses.ToArray();
            row.Length.ShouldBe(2);

            row[0].Status.ShouldBe(StockManageStatus.NotEnoughQuantity);
            row[0].Quantity.ShouldBe(20m);
            row[0].LocationId.ShouldBe(1);

            row[1].Status.ShouldBe(StockManageStatus.NotEnoughQuantity);
            row[1].Quantity.ShouldBe(20m);
            row[1].LocationId.ShouldBe(2);

            VerifyMoq();
        }
        [Fact]
        public async Task DecreaseAsync_StockNotExists_ReturnStocksNotExistsStatusWithInputData()
        {
            //Arrange

            Stock stock = null;
            SetupStock(stock);
            SetupProduct(true);

            //Act

            var result = await DecreaseAsync(10m);

            //Assert

            var row = result.ShouldHaveSingleItem().ResolveStatuses.ShouldHaveSingleItem();
            row.Status.ShouldBe(StockManageStatus.StocksNotFound);
            row.LocationId.ShouldBeNull();
            row.Quantity.ShouldBe(10m);

            VerifyMoq();
        }
        [Fact]
        public async Task DecreaseAsync_ProductNotExists_ReturnObjectHasProductNotExistsStatusAndInputQuantity()
        {
            //arrange

            var stock = CreateStock(1,10,2);
            SetupStock(stock);
            SetupProduct(false);

            //act
            var result = await DecreaseAsync(10m);

            //Assert

            var row = result.ShouldHaveSingleItem().ResolveStatuses.ShouldHaveSingleItem();
            row.Status.ShouldBe(StockManageStatus.ProductNotFound);
            row.Quantity.ShouldBe(10m);

            VerifyMoq();
        }
    }
}
