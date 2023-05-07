using System.Text.Json;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace Bank.Currency.Exchange.Api.Middleware;

public class LimitRequestsMiddleware : ClientRateLimitMiddleware
{
    public LimitRequestsMiddleware(RequestDelegate next, IProcessingStrategy processingStrategy,
        IOptions<ClientRateLimitOptions> options, IClientPolicyStore policyStore, IRateLimitConfiguration config,
        ILogger<ClientRateLimitMiddleware> logger) : base(next, processingStrategy, options, policyStore, config,
        logger)
    {
    }

    public override Task ReturnQuotaExceededResponse
        (HttpContext httpContext, RateLimitRule rule, string retryAfter)
    {
        var path = httpContext?.Request?.Path.Value;
        var result = JsonSerializer.Serialize("Your 10 free request were exceeded, try later!");
        httpContext.Response.Headers["Retry-After"] = retryAfter;
        httpContext.Response.StatusCode = 429;
        httpContext.Response.ContentType = "application/json";

        return httpContext.Response.WriteAsync(result);
    }
}