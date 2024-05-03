using System.Net;

namespace NZWalks.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> logger;
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {


            // log the exception
            var errorId = Guid.NewGuid();
            logger.LogError(ex, $"{errorId}: {ex.Message}");

            // return a custom error response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new
            {
                Id = errorId,
                ErrorMessage = "Something unexpected happened. We are fixing it."
            };
            await context.Response.WriteAsJsonAsync(error);
        }
    }

}