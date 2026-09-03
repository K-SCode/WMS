using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived
{
    public class ImportFileGoodsReceivedCommandValidator : AbstractValidator<ImportFileGoodsReceivedCommand>
    {
        public ImportFileGoodsReceivedCommandValidator()
        {
            RuleFor(f => f.Format)
                .NotEmpty()
                .NotNull();
            RuleFor(f => f.FileStream)
                .NotNull();
            RuleFor(f => f.FileStream.Length)
                .GreaterThan(0);

        }
    }
}
