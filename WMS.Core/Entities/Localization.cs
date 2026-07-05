using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class Localization
    {
        public int Id { get; set; }
        public char Sector { get; set; }
        public int Rack { get; set; }
        public int Number {  get; set; }
        public bool IsFull { get; set; } = false;
        public List<Stock> Stocks { get; set; } = new();
    }
}
