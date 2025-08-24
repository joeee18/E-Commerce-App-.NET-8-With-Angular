using Azure;
using ECom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Net;
using System.Text.Json;

namespace ECom.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(10);  


        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment; // Initialize the environment field
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);
                if (IsRequestedAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiExceptions((int)HttpStatusCode.TooManyRequests, "Rate limit exceeded. Please try again later.");
                    await context.Response.WriteAsJsonAsync(response);


                    //var json = JsonSerializer.Serialize(response);
                    //await context.Response.WriteAsync(json);
                    //return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner: " + ex.InnerException.Message;
                }

                var response = _environment.IsDevelopment()?
                    new ApiExceptions((int)HttpStatusCode.InternalServerError, errorMessage, ex.StackTrace):
                    new ApiExceptions((int)HttpStatusCode.InternalServerError, errorMessage);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private bool IsRequestedAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var cacheKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;
            var (timesTamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (TimesTamp: dateNow, count: 0);
            });
            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }
                _memoryCache.Set(cacheKey, (timesTamp, count += 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (dateNow, count), _rateLimitWindow);
            }
            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        }
    }
}