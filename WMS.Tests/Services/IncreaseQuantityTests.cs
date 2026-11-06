using Moq;
using Shouldly;
using WMS.Application.common.Service;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.Services
{
    public class IncreaseQuantityTests
    {
        private readonly Mock<IUnitOfWork> _mock;
        private readonly IncreaseQunatity _sut;

        private const string Barcode = "1231231231";

        public IncreaseQuantityTests()
        {
            _mock = new Mock<IUnitOfWork>();
            _sut = new IncreaseQunatity(_mock.Object);
        }

        private sealed record Model(string Barcode, decimal Quantity);
        private static Stock CreateStock(int locationId,decimal quantity, int productId)
        {
            return new Stock
            {
                LocationId = locationId,
                ProductId = productId,
                Quantity = quantity,
                MaxQuantity = 100
            };
        }

        private void SetupProduct(bool exists)
        {
            var map = new Dictionary<string, bool>
            {
                [Barcode] = exists
            };

            _mock.Setup(
                u => u.Product.ExistsByBarcodes(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(map);
        }
        private void SetupStock(params Stock[] stocks)
        {
            var map = new Dictionary<string, IReadOnlyCollection<Stock>>
               {
                   [Barcode] = stocks
               };
            _mock.Setup(
                p => p.Stock.GetNotFullStocksByProductBarcodesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(map);
        }
        private async Task<IReadOnlyCollection<StockResult>> Increase(decimal quantity)
        {
            return await _sut.IncreaseAsync(
                [new Model(Barcode, quantity)],
                p => p.Barcode,
                p => p.Quantity,
                CancellationToken.None
                );
        }
        private void VerifyMock()
        {
            _mock.Verify(u => u.Product.ExistsByBarcodes(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once()
                );
            _mock.Verify(u => u.Stock.GetNotFullStocksByProductBarcodesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once()
            );
            _mock.Verify(u => u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Fact]
        public async Task IncreaseAsync_QuantityHasEnoughPlaceInOneStocks_StockQuantityIncreaseAndStatusAccepted()
        {
            //Arrange

            var stock = CreateStock(1, 50m, 1);
            SetupProduct(true);
            SetupStock(stock);


            //Act
            var result = await Increase(10m);

            //Assert
            var row = result.ShouldHaveSingleItem()!.ResolveStatuses.ShouldHaveSingleItem();
            row.LocationId.ShouldBe(1);
            row.Status.ShouldBe(StockManageStatus.Accepted);
            row.Quantity.ShouldBe(10m);
            stock.Quantity.ShouldBe(60m);

            VerifyMock();
        }

        [Fact]
        public async Task IncreaseAsync_QuantityHasEnoughPlaceInFewStocks_StockIncreaseAndStatusIsAccepted()
        {
            //Arrange


            var stock1 = CreateStock(1, 80m, 1);
            var stock2 = CreateStock(2, 10m, 1);

            SetupStock([stock1, stock2]);
            SetupProduct(true);

            //Act
            var result = await Increase(60m);

            //Assert
            var row = result.ShouldHaveSingleItem().ResolveStatuses.ToArray();
            row.Length.ShouldBe(2);
            row[0].LocationId.ShouldBe(1);
            row[0].Status.ShouldBe(StockManageStatus.Accepted);
            row[0].Quantity.ShouldBe(20m);
            stock1.Quantity.ShouldBe(100m);

            row[1].LocationId.ShouldBe(2);
            row[1].Status.ShouldBe(StockManageStatus.Accepted);
            row[1].Quantity.ShouldBe(40m);
            stock2.Quantity.ShouldBe(50m);

            VerifyMock();
        }
        [Fact]
        public async Task IncreaseAsync_HasNotEnoughPlace_PartOfQuantityIsIncreaseAndHasPartiallyAcceptedStatusRestHasOutOfSpaceStatusWithNullLocation()
        {
            //Arrange

            var stock = CreateStock(1, 80m, 1);
            SetupProduct(true);
            SetupStock(stock);

            //Act
            var result = await Increase(60m);

            //Assert
            var row = result.ShouldHaveSingleItem().ResolveStatuses.ToArray();
            row[0].LocationId.ShouldBe(1);
            row[0].Status.ShouldBe(StockManageStatus.PartiallyAccepted);
            row[0].Quantity.ShouldBe(20m);
            stock.Quantity.ShouldBe(100m);

            row[1].LocationId.ShouldBeNull();
            row[1].Status.ShouldBe(StockManageStatus.OutOfSapce);
            row[1].Quantity.ShouldBe(40m);
            VerifyMock();
        }
        [Fact]
        public async Task IncreaseAsync_OutOfSpace_RetrunStockWithNullLocationAndOutOfSpaceStatus()
        {
            //Arrange
            
            var stock = CreateStock(1, 100m, 1);
            SetupStock(stock);
            SetupProduct(true);

            //Act
            var result = await Increase(60m);

            //Assert
            var row = result.ShouldHaveSingleItem().ResolveStatuses.ShouldHaveSingleItem();
            row.LocationId.ShouldBeNull();
            row.Status.ShouldBe(StockManageStatus.OutOfSapce);
            row.Quantity.ShouldBe(60m);
            stock.Quantity.ShouldBe(100m);
            VerifyMock();
        }
        [Fact]
        public async Task IncreaseAsync_ProductNotExists_ReturnProductNotFoundWithEnterData()
        {
            //arrange
            var stock = CreateStock(1, 50m, 1);
            SetupStock(stock);
            SetupProduct(false);

            //Act
            var resul = await Increase(20m);

            //Assert
            var row = resul.ShouldHaveSingleItem().ResolveStatuses.ShouldHaveSingleItem();
            row.Quantity.ShouldBe(20m);
            row.Status.ShouldBe(StockManageStatus.ProductNotFound);
            row.LocationId.ShouldBeNull();
            VerifyMock();
        }
    }
}