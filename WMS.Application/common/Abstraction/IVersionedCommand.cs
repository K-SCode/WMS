namespace WMS.Application.common.Abstraction
{
    public interface IVersionedCommand
    {
        byte[] Version {  get; }
    }
}
