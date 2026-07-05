using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public int LocalizationId { get; set; }
        public Localization Localization { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Count { get; set; }
    }
}
