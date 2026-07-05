using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<Stock> Stocks { get; set; } = new();
    }
}
