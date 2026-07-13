using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Features.Products.Dtos
{
    public record ProductDto(int Id, string Name, decimal Price, IEnumerable<Stock> Stocks);
}
