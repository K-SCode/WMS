using Moq;
using Shouldly;
using WMS.Application.Features.Orders.Command.CreateOrder;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using Xunit;

namespace WMS.Tests.Orders
{
    public class CreateOrderHandlerTests
    {
        private readonly CreateOrderHandler _sut;
        private readonly Mock<ICreateOrderProcess> _createOrderProcess;
        public CreateOrderHandlerTests()
        {
            _createOrderProcess = new Mock<ICreateOrderProcess>();
            _sut = new CreateOrderHandler(_createOrderProcess.Object);
        }

        private static CreateOrderCommand CreateCommand(string nip)
        {
            var orderInformation = new OrderInformation("test test 123", PaymentMethod.Card, Currency.EUR);
            var customerInforamtion = new CustomerInformation("test", "test", "123123123", nip, "test corp", "test@test.pl");
            IReadOnlyCollection<CreateProductOrder> items = [];
            return new CreateOrderCommand(orderInformation, customerInforamtion, items);
        }

        [Theory]
        [InlineData("1234563219")]
        [InlineData("5260210210")]
        [InlineData("7740001450")]
        [InlineData("1060000067")]
        [InlineData("9540000011")]
        public async Task Hanlde_InvalidNipVeryfication_ShouldReturnError(string nip)
        {
            //assert
            var command = CreateCommand(nip);

            //act
            var result = await _sut.Handle(command, CancellationToken.None);

            //assert
            result.IsSuccess.ShouldBe(false);
            result.Error.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("1234563218")]
        [InlineData("5260210211")]
        [InlineData("7740001454")]
        [InlineData("1060000062")]
        [InlineData("9540000014")]
        public async Task Hanlde_ValidNipVeryfication_ShouldReturnId(string nip)
        {
            //assert
            int expectedId = 10;
            CreateOrderCommand command = CreateCommand(nip);

            _createOrderProcess.Setup(
                o => o.ProcessAsync(
                    It.IsAny<CustomerInformation>(),
                    It.IsAny<IReadOnlyCollection<CreateProductOrder>>(),
                    It.IsAny<OrderInformation>(),
                    It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(expectedId);

            //act
            var result = await _sut.Handle(command, CancellationToken.None);

            //assert
            result.IsSuccess.ShouldBe(true);
            result.Value.ShouldBe(expectedId);

            _createOrderProcess.Verify(
                o => o.ProcessAsync(
                    It.IsAny<CustomerInformation>(),
                    It.IsAny<IReadOnlyCollection<CreateProductOrder>>(),
                    It.IsAny<OrderInformation>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

    }
}
