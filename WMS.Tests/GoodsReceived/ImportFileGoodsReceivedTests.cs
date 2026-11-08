using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shouldly;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces.GoodsReceipts;
using WMS.Application.Interfaces.Import;
using WMS.Core.Common;
using Xunit;

namespace WMS.Tests.GoodsReceived
{
    public class ImportFileGoodsReceivedTests
    {
        private readonly Mock<IProductImportFileAdapterFactory> _adapterFactory;
        private readonly Mock<IProductImportFileAdapter> _adapter;
        private readonly Mock<IGoodsReceivedProcess> _goodsReceivedProcess;
        private readonly Mock<IValidator<ProductReceived>> _validator;
        private readonly ImportFileGoodsReceivedCommandHandler _sut;
        public ImportFileGoodsReceivedTests()
        {
            _adapterFactory = new Mock<IProductImportFileAdapterFactory>();
            _goodsReceivedProcess = new Mock<IGoodsReceivedProcess>();
            _adapter = new Mock<IProductImportFileAdapter>();
            _validator = new Mock<IValidator<ProductReceived>>();
            _sut = new ImportFileGoodsReceivedCommandHandler(
                _adapterFactory.Object,
                _goodsReceivedProcess.Object,
                _validator.Object);
        }
        private static ImportFileGoodsReceivedCommand CreateCommand()
        {
            using var stream = new MemoryStream();
            var supplier = new SupplierInformation("1231231231", "test corp", "123123123", "test test tes");
            var documentInfomration = new DocumentInformation(supplier, "Fav-123123", "test comment");

            return new ImportFileGoodsReceivedCommand(stream, "csv", documentInfomration);
        }
        [Fact]
        public async Task Handle_IncorrectDataInFile_ReturnError()
        {
            //arrange
            var invalidItem = new ProductReceived("1111111111111", 0m, "szt", 5.50m);
            var command = CreateCommand();
            IReadOnlyCollection<ProductReceived> imported = [invalidItem];

            _adapterFactory.Setup(
                a => a.Create(It.IsAny<string>()))
                .Returns(_adapter.Object);
            _adapter.Setup(
                a => a.ParseAsync(It.IsAny<Stream>()))
                .ReturnsAsync(imported);

            _validator.Setup(
                v => v.ValidateAsync(
                    It.IsAny<ProductReceived>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult([new ValidationFailure()]));
            //act
            var result = await _sut.Handle(command,CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBe(false);
            result.Error.ShouldNotBeNull();
            result.Error.Type.ShouldBe(ErrorType.Failure);

            _goodsReceivedProcess.Verify(
                p => p.ProcessAsync(
                    It.IsAny<GoodsReceivedNote>(),
                    It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Fact]
        public async Task Handle_CorrectDataInFile_PassValidationTest()
        {
            //arrange
            var validItem = new ProductReceived("1231231231231", 10m, "szt", 5.50m);
            var command = CreateCommand();
            IReadOnlyCollection<ProductReceived> imported = [validItem];

            _adapterFactory.Setup(
                a => a.Create(It.IsAny<string>()))
                .Returns(_adapter.Object);
            _adapter.Setup(
                a => a.ParseAsync(It.IsAny<Stream>()))
                .ReturnsAsync(imported);

            _validator.Setup(
                v => v.ValidateAsync(
                    It.IsAny<ProductReceived>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            //act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBe(true);
            result.Error.ShouldBeNull();

            _goodsReceivedProcess.Verify(
                p => p.ProcessAsync(
                    It.IsAny<GoodsReceivedNote>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

    }
}
