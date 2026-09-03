using Mediator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived
{
    public sealed record ImportFileGoodsReceivedCommand(Stream FileStream, string Format) :
        ICommand<Result<Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>>>>;
}
