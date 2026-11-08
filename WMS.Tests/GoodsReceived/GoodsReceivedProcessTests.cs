using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Shouldly;
using WMS.Application.common.Service;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Features.GoodsReceived.Services;
using WMS.Application.Interfaces.GoodsReceipts;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Stocks;
using WMS.Application.Interfaces.Warehouse;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.GoodsReceived
{
    public class GoodsReceivedProcessTests
    {
        private readonly Mock<IIncreaseQuantity> _increaseQuantity;
        private readonly Mock<ISaveGoodsReceipt> _saveReceipt;
        private readonly Mock<ICreateWarehouseTask> _createWarehouseTask;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IDbContextTransaction> _transaction;
        private readonly GoodsReceivedProcess _sut;
        private List<string> _calls;
        public GoodsReceivedProcessTests()
        {
            _increaseQuantity = new Mock<IIncreaseQuantity>();
            _saveReceipt = new Mock<ISaveGoodsReceipt>();
            _createWarehouseTask = new Mock<ICreateWarehouseTask>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _transaction = new Mock<IDbContextTransaction>();

            _sut = new GoodsReceivedProcess(
                _increaseQuantity.Object,
                _saveReceipt.Object,
                _createWarehouseTask.Object,
                _unitOfWork.Object);

            _calls = new();
        }
        [Fact]
        public async Task ProcessAsync_AllSetpsSucceedInStepsOrderCreatesOverflowTasksAndCommits_ReturnTrue()
        {
            int grnId = 20;
            IReadOnlyCollection<ProductReceived> items = [new ProductReceived("1231231231231", 10m, "szt", 100m)];
            var supplier = new SupplierInformation("1231231231","Corp test","123123123","test test");
            var documentInformation = new DocumentInformation(supplier, "FV-32131", "test test test");
            var goodsReceived = new GoodsReceivedNote(items, documentInformation);
            IReadOnlyCollection<StockResult> stockResult = [new ("1231231231231", [new ResolveStatus(1, StockManageStatus.Accepted, 10m)])];
            var grn = new GoodsReceipt { Id = grnId };
         
            _increaseQuantity.Setup(
                i => i.IncreaseAsync(
                    It.IsAny<IReadOnlyCollection<ProductReceived>>(),
                    It.IsAny<Func<ProductReceived, string>>(),
                    It.IsAny<Func<ProductReceived, decimal>>(),
                    It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(IIncreaseQuantity.IncreaseAsync)))
                .ReturnsAsync(stockResult);

            _saveReceipt.Setup(
                s => s.SaveInDatabaseAsync(
                    It.IsAny<IReadOnlyCollection<ProductReceived>>(),
                    It.IsAny<DocumentInformation>(),
                    It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(ISaveGoodsReceipt.SaveInDatabaseAsync)))
                .ReturnsAsync(grn);

            _createWarehouseTask.Setup(
                w => w.CreateTaskAsync(
                    It.IsAny<IReadOnlyCollection<StockResult>>(),
                    It.IsAny<WarehouseSource>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(ICreateWarehouseTask.CreateTaskAsync)))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(IUnitOfWork.BeginTransactionAsync)))
                .ReturnsAsync(_transaction.Object);

            _unitOfWork.Setup(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(IUnitOfWork.SaveChangesAsync)))
                .Returns(Task.CompletedTask);

            _transaction.Setup(
                t => t.CommitAsync(It.IsAny<CancellationToken>()))
                .Callback(() => _calls.Add(nameof(IDbContextTransaction.CommitAsync)))
                .Returns(Task.CompletedTask);

            //act
            var result = await _sut.ProcessAsync(goodsReceived, CancellationToken.None);

            //Assert

            result.ShouldBe(true);

            _calls.ShouldBe(new[]
            {
                nameof(IUnitOfWork.BeginTransactionAsync),
                nameof(ISaveGoodsReceipt.SaveInDatabaseAsync),
                nameof(IUnitOfWork.SaveChangesAsync),
                nameof(IIncreaseQuantity.IncreaseAsync),
                nameof(ICreateWarehouseTask.CreateTaskAsync),
                nameof(IUnitOfWork.SaveChangesAsync),
                nameof(IDbContextTransaction.CommitAsync)
            });

            _createWarehouseTask.Verify(
                w => w.CreateTaskAsync(
                    stockResult,
                    WarehouseSource.GoodsReceiptOverflow,
                    grnId,
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }
    }
}
