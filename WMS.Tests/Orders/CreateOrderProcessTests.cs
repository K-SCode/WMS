
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Shouldly;
using WMS.Application.common.Service;
using WMS.Application.Features.Orders.Command.Service;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Application.Interfaces.Stocks;
using WMS.Application.Interfaces.Warehouse;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using Xunit;

namespace WMS.Tests.Orders
{
    public class CreateOrderProcessTests
    {
        private readonly Mock<IAssignCustomerOrder> _assingCustomerOrder;
        private readonly Mock<IOrderRegistration> _orderRegistration;
        private readonly Mock<IAssignOrderItems> _assingOrderItems;
        private readonly Mock<IDecreaseQuantity> _decreaseQuantity;
        private readonly Mock<ICreateWarehouseTask> _createWarehouseTask;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IDbContextTransaction> _transaction;
        private readonly CreateOrderProcess _sut;
        public CreateOrderProcessTests()
        {
            _assingCustomerOrder = new Mock<IAssignCustomerOrder>();
            _orderRegistration = new Mock<IOrderRegistration>();
            _assingOrderItems = new Mock<IAssignOrderItems>();
            _decreaseQuantity = new Mock<IDecreaseQuantity>();
            _createWarehouseTask = new Mock<ICreateWarehouseTask>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _transaction = new Mock<IDbContextTransaction>();

            _sut = new CreateOrderProcess(
                _assingCustomerOrder.Object,
                _orderRegistration.Object,
                _assingOrderItems.Object,
                _decreaseQuantity.Object,
                _createWarehouseTask.Object,
                _unitOfWork.Object);
        }
        private void AssingItemsToOrder(bool isSuccess)
        {
            _assingOrderItems.Setup(o => o.Assing(
                            It.IsAny<IReadOnlyCollection<OrderItem>>(),
                            It.IsAny<Order>(),
                            It.IsAny<CancellationToken>()))
                            .ReturnsAsync(isSuccess);
        }

        private void DecreaseSetup(IReadOnlyCollection<StockResult> decreased)
        {
            _decreaseQuantity.Setup(
                            d => d.DecreaseAsync(
                                It.IsAny<IReadOnlyCollection<(Product, decimal)>>(),
                                It.IsAny<Func<(Product, decimal), string>>(),
                                It.IsAny<Func<(Product, decimal), decimal>>(),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync(decreased);
        }

        private void UnitOfWorksSetup(IReadOnlyCollection<Product> productsInDb)
        {
            _unitOfWork.Setup(
                            u => u.Product.GetAllByIdsAsync(
                                It.IsAny<IReadOnlyCollection<int>>(),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync(productsInDb);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_transaction.Object);
        }

        private void OrderRegisterSetup(Order order)
        {
            _orderRegistration.Setup(
                            r => r.RegisterAsync(
                                It.IsAny<Order>(),
                                It.IsAny<Customer>(),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync(order);
        }

        private void AssignCustomerToOrderSetup(Customer customer)
        {
            _assingCustomerOrder.Setup(
                                        c => c.AssignAsync(
                                            It.IsAny<Customer>(),
                                            It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(customer);
        }

        private static OrderInformation CreateOrderInformation()
        {
            return new OrderInformation(
                "abc 213, USA",
                PaymentMethod.Card,
                Currency.PLN);
        }

        private static Customer CreateCustomer(CustomerInformation customerInformation)
        {
            return new Customer { Id = 1, Email = customerInformation.Email };
        }

        private static CustomerInformation CreateCustomerInformation(string nip, string companyName)
        {
            return new CustomerInformation(
                "John",
                "Smith",
                "123456789",
                nip,
                companyName,
                "Sample@Sample.com");
        }

        [Theory]
        [InlineData("1231231231", "Simple company Name")]
        [InlineData(null,null)]
        public async Task ProcessAsync_CorrectTransaction_ReturnId(string nip, string companyName)
        {
            //Arrange
            int productId = 10;
            int expectedOrderId = 7;
            string barcode = "1231231231231";

            var customerInformation = CreateCustomerInformation(nip, companyName);
            var customer = CreateCustomer(customerInformation);
            var orderInfo = CreateOrderInformation();

            IReadOnlyCollection<CreateProductOrder> items = [new(productId, 10m)];

            Order order = new() { Id = expectedOrderId };
            IReadOnlyCollection<Product> productsInDb = [new Product { Id = productId, Barcode = barcode }];
            IReadOnlyCollection<StockResult> decreased = [
                new StockResult(
                    barcode,
                    [new ResolveStatus(1, StockManageStatus.Accepted, 10m)])];

            AssignCustomerToOrderSetup(customer);
            OrderRegisterSetup(order);

            UnitOfWorksSetup(productsInDb);
            DecreaseSetup(decreased);
            AssingItemsToOrder(true);

            //Act
            var result = await _sut.ProcessAsync(
                customerInformation,
                items,
                orderInfo,
                CancellationToken.None);

            //assert

            result.Value.ShouldBe(expectedOrderId);

            _transaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
            _createWarehouseTask.Verify(
                w => w.CreateTaskAsync(
                    decreased,
                    WarehouseSource.NewOrder,
                    expectedOrderId,
                    It.IsAny<CancellationToken>()),
                Times.Once()
                );
        }


        [Fact]
        public async Task ProcessAsync_UnacceptedStatus_ReturnError()
        {
            //Arrange
            int productId = 10;
            string barcode = "1231231231231";
            var customerInformation = CreateCustomerInformation(null,null);
            var customer = CreateCustomer(customerInformation);
            var orderInformation = CreateOrderInformation();
            IReadOnlyCollection<CreateProductOrder> items = [new(productId, 10m)];
            IReadOnlyCollection<Product> productsInDb = [new Product { Id = productId ,Barcode = barcode}];
            IReadOnlyCollection<StockResult> decreased = [
                new StockResult(
                    barcode,
                    [new ResolveStatus(1, StockManageStatus.OutOfSapce, 10m)])];

            UnitOfWorksSetup(productsInDb);
            DecreaseSetup(decreased);
            //Act

            var result = await _sut.ProcessAsync(
                customerInformation,
                items,
                orderInformation,
                CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBe(false);
            result.Error.ShouldNotBeNull();

            _createWarehouseTask.Verify(
                w => w.CreateTaskAsync(
                    It.IsAny<IReadOnlyCollection<StockResult>>(),
                    It.IsAny<WarehouseSource>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Never());

            _transaction.Verify(
                t => t.CommitAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never());

        }
        [Fact]
        public async Task ProcessAsync_ExceptionDuringProcess_TransactionBreakProcess()
        {
            //Arrange
            int productId = 10;
            string barcode = "1231231231231";
            var customerInformation = CreateCustomerInformation(null, null);
            var customer = CreateCustomer(customerInformation);
            var orderInformation = CreateOrderInformation();
            IReadOnlyCollection<CreateProductOrder> items = [new(productId, 10m)];
            IReadOnlyCollection<Product> productsInDb = [new Product { Id = productId, Barcode = barcode }];

            UnitOfWorksSetup(productsInDb);
            _unitOfWork.Setup(
                u => u.Product.GetAllByIdsAsync(
                    It.IsAny<IReadOnlyCollection<int>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Assert
            await Should.ThrowAsync<Exception>(async () =>
            {
                //act
                var result = await _sut.ProcessAsync(customerInformation, items, orderInformation, CancellationToken.None);
            });
            _transaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

    }
}

