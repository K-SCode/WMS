using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.Orders.Command.CancelOrder;
using WMS.Application.Features.Orders.Command.CreateOrder;
using WMS.Application.Features.Orders.Command.ReturnOrder;
using WMS.Application.Features.Orders.Command.ShippedOrder;
using WMS.Application.Features.Orders.Query.Get;
using WMS.Application.Features.Orders.Query.List;
using WMS.Core.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policy.ManagerOrWarehouseman)]
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
        [HttpPut("[action]")]
        public async Task<IActionResult> Returned(ReturnOrderCommand command, CancellationToken cancellationToken)
        {
            var changeToReturned = await sender.Send(command, cancellationToken);
            return changeToReturned.ToOkOrProblem();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await sender.Send(new ListOrderQuery(), cancellationToken);
            return orders.ToOkOrProblem();
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] GetOrderQuery query, CancellationToken cancellationToken)
        {
            var order = await sender.Send(query, cancellationToken);
            return order.ToOkOrProblem();
        }
    }
}