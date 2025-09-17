using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LMS.API.Filters;

/// <summary>
/// This filter ensures that all model validation errors are handled consistently across the API.
/// It provides a standardized ProblemDetails response that is unified with the application's exception handling, improving client experience and maintainability.
/// </summary>
public class ValidationFilter : IActionFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ValidationFilter(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory;
    }

    // <summary>
    // Called before the action executes, checks for model state validity.
    // If invalid, it creates a ProblemDetails response and short-circuits the action execution.
    // </summary>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var problemDetails = _problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation Failed",
                detail: "One or more validation errors occurred.",
                instance: context.HttpContext.Request.Path
            );

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed here
    }
}