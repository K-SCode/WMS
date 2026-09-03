using WMS.Application.Interfaces;

namespace WMS.Infrastructure.Import
{
    public class ProductImportFileFactory(IEnumerable<IProductImportFileAdapter> adapters) : IProductImportFileAdapterFactory
    {
        public IProductImportFileAdapter? Create(string format)
        {

            var adapter = adapters.FirstOrDefault(a => string.Equals(a.Format, format, StringComparison.OrdinalIgnoreCase));
            return adapter;
        }
    }
}
