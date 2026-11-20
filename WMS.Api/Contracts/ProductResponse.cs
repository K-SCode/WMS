namespace WMS.Api.Contracts
{
    public record ProductResponse(
      int Id,
      string Name,
      decimal Price,
      string Barcode,
      string UnitQuantity);
}
