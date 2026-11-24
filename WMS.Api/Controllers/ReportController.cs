using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WMS.Api.Contracts;
using WMS.Api.Extensions;
using WMS.Application.Features.Raports.Query.GetMostProfitable;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = Roles.Analyst)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class ReportController(ISender sender) : ControllerBase
    {
        [HttpGet("most-profitable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MostProfitableItems([FromQuery] MostProfitableItemsRequest request, CancellationToken cancellationToken)
        {
            if (GetDate(request.FromDate, out DateTime minDate) ||
                GetDate(request.ToDate, out DateTime maxDate))
            {
                return Result.Failure(Errors.InvalidDataFormat).ToOkOrProblem();
            }
            var query = new ListMostProfitableQuery(minDate, maxDate);

            var result = await sender.Send(query, cancellationToken);

            return result.ToOkOrProblem();
        }

        private static bool GetDate(string date, out DateTime minDate)
        {
            return !DateTime.TryParseExact(
                date,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out minDate);
        }
    }
}
