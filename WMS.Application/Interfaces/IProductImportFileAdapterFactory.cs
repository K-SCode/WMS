namespace WMS.Application.Interfaces
{
    public interface IProductImportFileAdapterFactory
    {
        public IProductImportFileAdapter? Create(string format);
    }
}
