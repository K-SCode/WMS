namespace WMS.Api.Contracts
{
    public sealed record UpdateLocationRequest(char Sector, int Rack, int Number);
}
