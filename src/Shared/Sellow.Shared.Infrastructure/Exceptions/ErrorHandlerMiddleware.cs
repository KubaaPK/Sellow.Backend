using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Infrastructure.Exceptions;

internal sealed class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);

            await HandleError(context, exception);
        }
    }

    private static async Task HandleError(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "server_error";
        var message = "Whops! Something went wrong.";

        if (exception is SellowException sellowException)
        {
            statusCode = sellowException.HttpCode;
            errorCode = sellowException.ErrorCode;
            message = sellowException.Message;
        }

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(new
        {
            statusCode,
            errorCode,
            message
        });
    }
}