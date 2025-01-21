using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _fullAccessKey;
    private readonly string _getAccessKey;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
        _fullAccessKey = Environment.GetEnvironmentVariable("FULL_ACCESS_KEY");
        _getAccessKey = Environment.GetEnvironmentVariable("GET_ACCESS_KEY");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        if (extractedApiKey == _fullAccessKey)
        {
            await _next(context);
        }
        else if (extractedApiKey == _getAccessKey)
        {
            if (context.Request.Method != HttpMethods.Get)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Unauthorized client");
            }
            else
            {
                await _next(context);
            }

        }
        else
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized client");
        }
    }
}
