using Microsoft.AspNetCore.Mvc;
using WMS.Core.Common;

namespace WMS.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToOkOrProblem(this Result result) =>
            result.IsSuccess ? new OkResult() : CreateProblem(result.Error!);

        public static IActionResult ToOkOrProblem<T>(this Result<T> result) =>
            result.IsSuccess ? new OkObjectResult(result.Value) : CreateProblem(result.Error!);

        public static IActionResult ToNoContentOrProblem(this Result result) =>
            result.IsSuccess ? new NoContentResult() : CreateProblem(result.Error!);

        public static IActionResult ToCreatedAtActionOrProblem<T>(
            this Result<T> result,
            string actionName,
            string controllerName)
        {
            if (!result.IsSuccess)
                return CreateProblem(result.Error!);

            var cleanControllerName = controllerName?.Replace("Controller", string.Empty);

            return new CreatedAtActionResult(
                actionName,
                cleanControllerName,
                new { id = result.Value },
                result.Value);
        }

        private static ObjectResult CreateProblem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
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
