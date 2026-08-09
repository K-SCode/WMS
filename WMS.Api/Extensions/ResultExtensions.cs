using Microsoft.AspNetCore.Mvc;
using WMS.Core.Common;

namespace WMS.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result) =>
            result.IsSuccess ? new OkResult() : CreateProblem(result.Error!);

        public static IActionResult ToActionResult<T>(this Result<T> result) =>
            result.IsSuccess ? new OkObjectResult(result.Value) : CreateProblem(result.Error!);

        private static ObjectResult CreateProblem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(
                new ProblemDetails
                {
                    Status = statusCode,
                    Title = error.Title,
                    Detail = error.Description
                }
            )
            {
                StatusCode = statusCode
            };
        }
    }
}
