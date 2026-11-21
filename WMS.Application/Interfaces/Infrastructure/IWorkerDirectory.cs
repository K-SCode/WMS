namespace WMS.Application.Interfaces.Infrastructure
{
    public interface IWorkerDirectory
    {
        Task<IReadOnlyCollection<int>> GetActiveWarehousemenIdsAsync(CancellationToken ct);
    }
}
