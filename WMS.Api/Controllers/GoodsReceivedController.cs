using Mediator;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Extensions;
using WMS.Application.Features.GoodsReceived.Commands.GoodsReceived;
using WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived;
using WMS.Application.Features.GoodsReceived.Dto;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsReceivedController(ISender sender) : ControllerBase
    {
        [HttpPut("[action]")]
        public async Task<IActionResult> GoodsReceived([FromBody] GoodsReceivedCommand goodsReceived, CancellationToken cancellationToken)
        {
            var goodReceiptConfirmation = await sender.Send(goodsReceived, cancellationToken);

            return goodReceiptConfirmation.ToOkOrProblem();
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportGoodsRecived([FromForm] DocumentInformation information, IFormFile file, CancellationToken cancellationToken)
        {

            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
            await using Stream fileStream = file.OpenReadStream();
            
            ImportFileGoodsReceivedCommand command = new(fileStream, extension,information);
            var result = await sender.Send(command, cancellationToken);

            return result.ToOkOrProblem();
        }
    }
}
