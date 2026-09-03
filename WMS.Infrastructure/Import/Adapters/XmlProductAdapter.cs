using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Interfaces;

namespace WMS.Infrastructure.Import.Adapters
{
    public class XmlProductAdapter : IProductImportFileAdapter
    {
        public string Format => "xml";

        public async Task<IEnumerable<ProductReceived>> ParseAsync(Stream input)
        {
            var doc = await XDocument.LoadAsync(input, LoadOptions.None, CancellationToken.None);

            var records = doc.Descendants("Product")
                .Select(element => new ProductReceived(
                    Barcode: element.Element("Barcode")?.Value ?? string.Empty,
                    Quantity: int.TryParse(element.Element("Quantity")?.Value, out var quantity) ? quantity : 0
                ))
                .ToList();

            return records;
        }
    }
}