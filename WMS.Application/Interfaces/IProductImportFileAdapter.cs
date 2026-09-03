using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Commands;

namespace WMS.Application.Interfaces
{
    public interface IProductImportFileAdapter
    {
        string Format { get; }
        Task<IEnumerable<ProductReceived>> ParseAsync(Stream input);
    }
}
