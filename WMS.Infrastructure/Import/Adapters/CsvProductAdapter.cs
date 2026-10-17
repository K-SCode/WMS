using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Interfaces;

namespace WMS.Infrastructure.Import.Adapters
{
    public class CsvProductAdapter : IProductImportFileAdapter
    {
        public string Format => "csv";

        public Task<IEnumerable<ProductReceived>> ParseAsync(Stream input)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using var reader = new StreamReader(input);
            using var csv = new CsvReader(reader, config);

         
            List<ProductReceived> products = csv.GetRecords<ProductReceived>().ToList();

            return Task.FromResult<IEnumerable<ProductReceived>>(products);
        }
    }
}
