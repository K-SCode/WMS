using Moq;
using Shouldly;
using System.Net.NetworkInformation;
using WMS.Application.Features.Orders.Command.CancelOrder;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.Orders
{
    public class CancelOrderHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IOrderRestocking> _orderRestocking;
        private readonly CancelOrderHandler _sut;

        public CancelOrderHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _orderRestocking = new Mock<IOrderRestocking>();
            _sut = new CancelOrderHandler(_unitOfWork.Object,_orderRestocking.Object);
        }

        [Theory]
        [InlineData(OrderStatus.New)]
        [InlineData(OrderStatus.Cancelled)]
        public async Task Handler_ValidChangeStatusToCancel_ChangeStatusAndUpdateData(OrderStatus status)
        {
            //Arrange
            var command = new CancelOrderCommand(1);
            var order = new Order { Id = command.OrderId, Status = status };

            _unitOfWork.Setup(
                u => u.Order.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderRestocking.Setup(
                r => r.Restock(
                    It.IsAny<Order>(),
                    It.IsAny<WarehouseSource>(),
                    It.IsAny<CancellationToken>()));
            //act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBe(true);
            result.Value.ShouldBe(true);

            _unitOfWork.Verify(
                u => u.Order.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            _orderRestocking.Verify(
                 r => r.Restock(
                    It.IsAny<Order>(),
                    It.IsAny<WarehouseSource>(),
                    It.IsAny<CancellationToken>()),
                 Times.Once());
        }

        [Theory]
        [InlineData(OrderStatus.Returned)]
        [InlineData(OrderStatus.Shipped)]
        public async Task Handler_InvalidChangeStatusToCancel_ReturnError(OrderStatus status)
        {
            //Arrange
            var command = new CancelOrderCommand(1);
            var order = new Order { Id = command.OrderId, Status = status };

            _unitOfWork.Setup(
                u => u.Order.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            //act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBe(false);
            result.Error.ShouldNotBeNull();

            _unitOfWork.Verify(
                u => u.Order.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
            _unitOfWork.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never());

            _orderRestocking.Verify(
                 r => r.Restock(
                    It.IsAny<Order>(),
                    It.IsAny<WarehouseSource>(),
                    It.IsAny<CancellationToken>()),
                 Times.Never());
        }
    }
}
