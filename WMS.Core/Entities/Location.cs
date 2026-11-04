using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public char Sector { get; set; }
        public int Rack { get; set; }
        public int Number {  get; set; }
        public List<Stock> Stocks { get; set; } = new();
    }
}
