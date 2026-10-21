using Mediator;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Features.Orders.Command.CreateOrder;
using WMS.Api.Extensions;
using WMS.Application.Features.Orders.Command.CancelOrder;
using WMS.Application.Features.Orders.Command.ShippedOrder;
namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ISender sender) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody]CreateOrderCommand command,CancellationToken cancellationToken)
        {
            var id = await sender.Send(command, cancellationToken);
            return id.ToOkOrProblem();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Cancel(CancelOrderCommand command,CancellationToken cancellationToken)
        {
            var isCancel = await sender.Send(command, cancellationToken);
            return isCancel.ToOkOrProblem();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Shipped(ShippedOrderCommand command, CancellationToken cancellationToken)
        {
            var changeToShipped = await sender.Send(command, cancellationToken);

            return changeToShipped.ToOkOrProblem();
        }
    }
}