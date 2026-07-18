using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Products.Dtos
{
    public record ProductDto(int Id, string Name, decimal Price);
}
