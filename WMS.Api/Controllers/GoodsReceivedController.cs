using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.GoodsReceived.Commands.GoodsReceived;
using WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Features.GoodsReceived.Query.Get;
using WMS.Application.Features.GoodsReceived.Query.List;
using WMS.Core.Common;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoodsReceivedController(ISender sender) : ControllerBase
    {
        [HttpPut("[action]")]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> GoodsReceived([FromBody] GoodsReceivedCommand goodsReceived, CancellationToken cancellationToken)
        {
            var goodReceiptConfirmation = await sender.Send(goodsReceived, cancellationToken);

            return goodReceiptConfirmation.ToOkOrProblem();
        }
        [HttpPost("import")]
        [Authorize(Policy = Policy.ManagerOrWarehouseman)]
        public async Task<IActionResult> ImportGoodsRecived([FromForm] DocumentInformation information, IFormFile file, CancellationToken cancellationToken)
        {

            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
            await using Stream fileStream = file.OpenReadStream();
            
            ImportFileGoodsReceivedCommand command = new(fileStream, extension,information);
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }
        [HttpGet()]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var goodsReceiveds = await sender.Send(new ListGoodsReceivedQuery(), cancellationToken);
            return goodsReceiveds.ToOkOrProblem(); 
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> Get([FromRoute] GetGoodsReceivedQuery query, CancellationToken cancellationToken)
        {
            var goodsReceived = await sender.Send(query, cancellationToken);
            return goodsReceived.ToOkOrProblem();
        }
    }
}
