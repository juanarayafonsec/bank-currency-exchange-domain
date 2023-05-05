using System.Net;
using System.Text.Json;
using Bank.Currency.Exchange.Application.Exceptions;
using Bank.Currency.Exchange.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bank.Currency.Exchange.Application.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RegistrationException re)
        {
            _logger.LogError(re, "Registration Error");
            ContextSetup(context, (int)HttpStatusCode.BadRequest, re.Message);
        }
        catch (LoginException le)
        {
            _logger.LogError(le, "Login Error");
            ContextSetup(context, (int)HttpStatusCode.BadRequest,
                le.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled error exploded D: ");
            ContextSetup(context, (int)HttpStatusCode.InternalServerError,
                "Internal Server Error");
        }
    }

    private static async void ContextSetup(HttpContext context, int errorCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorCode;
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var response = new HttpException(errorCode, message);
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}