namespace WMS.Application.Interfaces.Import
{
    public interface IProductImportFileAdapterFactory
    {
        public IProductImportFileAdapter? Create(string format);
    }
}
