using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public int Id_localization { get; set; }
        public int Id_product { get; set; }
        public int Count { get; set; }
    }
}
