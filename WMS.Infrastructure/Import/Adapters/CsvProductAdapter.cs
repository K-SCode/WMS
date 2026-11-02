using CsvHelper;
using System.Globalization;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Interfaces;

namespace WMS.Infrastructure.Import.Adapters
{
    public class CsvProductAdapter : IProductImportFileAdapter
    {
        public string Format => "csv";

        public Task<IReadOnlyCollection<ProductReceived>> ParseAsync(Stream input)
        {
            var options = new CsvReaderOptions { Separator = ';' };
            using var reader = new StreamReader(input);
            using var csv = CsvReader.Create(reader, options);

         
            List<ProductReceived> products = csv.GetRecords<ProductReceived>().ToList();

            return Task.FromResult<IReadOnlyCollection<ProductReceived>>(products);
        }
    }
}
