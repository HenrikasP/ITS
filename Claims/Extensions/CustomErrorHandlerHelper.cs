using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Extensions;

public static class CustomErrorHandlerHelper
{
    public static void UseCustomErrors(this IApplicationBuilder app)
    {
        app.Use((HttpContext httpContext, Func<Task> _) => WriteResponse(httpContext));
    }

    private static async Task WriteResponse(HttpContext httpContext)
    {
        // Try and retrieve the error from the ExceptionHandler middleware
        var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
        var ex = exceptionDetails?.Error;

        // Should always exist, but best to be safe!
        if (ex != null)
        {
            // ProblemDetails has it's own content type
            httpContext.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = 500,
                Title = "An error occured",
                Detail = null
            };

            // This is often very handy information for tracing the specific request
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            if (!string.IsNullOrEmpty(traceId))
            {
                problem.Extensions["traceId"] = traceId;
            }

            //Serialize the problem details object to the Response as JSON (using System.Text.Json)
            var stream = httpContext.Response.Body;
            await JsonSerializer.SerializeAsync(stream, problem);
        }
    }
}