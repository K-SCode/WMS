namespace WMS.Api.Contracts
{
    public sealed record UpdateProductRequest(string Name, decimal Price, string UnityQuantity);
}
