using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.SharedLibrary
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            // Her isteğe özgü bir trace id oluştur
            var traceId = Guid.NewGuid().ToString();
            // var username = context.User.Identity.Name;
            //Console.WriteLine("username is : " + username);

            var stopwatch = Stopwatch.StartNew(); 
            
            context.Items["TraceId"] = traceId;

            // Log girişlerine trace id'yi ekleyerek ilerle
            //using (LogContext.PushProperty("Username", username))
            using (LogContext.PushProperty("TraceId", traceId))
            {
                _logger.LogInformation("TraceId: {TraceId} - Request started for {Method} {Path}", traceId, context.Request.Method, context.Request.Path);
                await _next(context);
                _logger.LogInformation("TraceId: {TraceId} - Request finished for {Method} {Path}", traceId, context.Request.Method, context.Request.Path);

                stopwatch.Stop(); 
            }

        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
