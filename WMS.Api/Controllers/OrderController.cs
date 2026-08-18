using Mediator;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.GoodsReceived;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ISender sender) : ControllerBase
    {
        [HttpPut("[action]")]
        public async Task<IActionResult> GoodsReceived([FromBody] GoodsReceivedCommand goodsReceived, CancellationToken cancellationToken)
        {
            var goodReceiptConfirmation = await sender.Send(goodsReceived, cancellationToken);

            return goodReceiptConfirmation.ToOkOrProblem();
        }
    }
}
