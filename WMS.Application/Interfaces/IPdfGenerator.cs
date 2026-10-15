using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces
{
    public interface IPdfGenerator<TModel>
    {
        void Generate(Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>> items);
    }
}
