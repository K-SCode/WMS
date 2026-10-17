using FluentValidation;
using Mediator;
using System;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Features.GoodsReceived.Services;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived
{
    public class ImportFileGoodsReceivedCommandHandler(
        IProductImportFileAdapterFactory productAdapterFactory,
        IGoodsReceivedProcess processor,
        IValidator<ProductReceived> validator) :
        ICommandHandler<ImportFileGoodsReceivedCommand, Result<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>>>
    {
        public async ValueTask<Result<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>>> Handle(
            ImportFileGoodsReceivedCommand command,
            CancellationToken cancellationToken)
        {
            var adapter = productAdapterFactory.Create(command.Format);

            if(adapter is null)
            {
                return Errors.InvalidFileFormat(command.Format);
            }

            IEnumerable<ProductReceived> importedItems = await adapter.ParseAsync(command.FileStream);

            (bool, string) validData = await ValidDataAsync(importedItems, cancellationToken);
            if (!validData.Item1)
            {
                return Errors<GoodsReceviedValidator>.ValidationProductFailure(validData.Item2);
            }

            GoodsReceivedNote goodsReceivedNote = new(importedItems,command.Information);

            var result = await processor.ProcessAsync(goodsReceivedNote, cancellationToken);
            return result;
        }
        private async Task<(bool,string)> ValidDataAsync(IEnumerable<ProductReceived> items,CancellationToken ct)
        {
            foreach (var item in items)
            {
                var validationResult = await validator.ValidateAsync(item, ct);
                if (!validationResult.IsValid)
                {
                    var message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return (false, message);
                }
            }
            return (true,string.Empty);
        }
    }
}
