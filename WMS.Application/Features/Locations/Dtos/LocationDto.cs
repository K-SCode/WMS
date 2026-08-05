using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Dtos
{
    public sealed record LocationDto(int Id, char Sector, int Rack, int Number, bool IsFull);
}
