using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LMS.API.Extensions;

public static class ExceptionMiddlewareExtetensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();
                    var environment = app.Services.GetRequiredService<IWebHostEnvironment>();

                    int statusCode;
                    string title;
                    string detail;

                    if (contextFeature.Error is AppException ex)
                    {
                        statusCode = (int)ex.StatusCode;
                        detail = ex.Message;
                        title = ex.Title ?? ex.GetType().Name.Replace("Exception", "");
                    }
                    else
                    {
                        statusCode = StatusCodes.Status500InternalServerError;
                        detail = environment.IsDevelopment()
                            ? contextFeature.Error.Message
                            : "An unexpected error occurred.";
                        title = "Internal Server Error";
                    }

                    var problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context,
                        statusCode,
                        title,
                        detail,
                        instance: context.Request.Path);

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}
