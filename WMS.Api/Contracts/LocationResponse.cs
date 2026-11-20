namespace WMS.Api.Contracts
{
    public class LocationResponse
    {
        public sealed record LocationDto(int Id, char Sector, int Rack, int Number);
    }
}
