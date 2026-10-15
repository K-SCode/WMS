using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Services
{
    internal class SaveGoodsReceiptService(IUnitOfWorks unitOfWorks)
    {
        public void SaveGoodsReceiptInDatabase(Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>> items)
        {
            
        }
    }
}
