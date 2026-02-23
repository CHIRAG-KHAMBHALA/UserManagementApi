using System.Diagnostics;

namespace UserManagementAPI.Middleware
{
    // Logs every request: HTTP method, path, status code, and how long it took
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Start the timer before passing down the pipeline
            var watch = Stopwatch.StartNew();

            await _next(context);

            watch.Stop();

            // Log after the response is done so we have the status code
            _logger.LogInformation(
                "[{Method}] {Path} => {StatusCode} ({Ms}ms)",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                watch.ElapsedMilliseconds
            );
        }
    }
}
