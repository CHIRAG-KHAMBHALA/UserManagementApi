namespace UserManagementAPI.Middleware
{
    // Protects write endpoints (POST, PUT, DELETE) with a simple API key check.
    // GET requests are public and pass through without any check.
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        // In a real project this would come from config/secrets, not hardcoded
        private const string ExpectedKey = "dev-secret";
        private const string HeaderName  = "X-API-KEY";

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // GETs are read-only, no key needed
            if (HttpMethods.IsGet(context.Request.Method))
            {
                await _next(context);
                return;
            }

            // All other methods require the header
            var hasKey = context.Request.Headers.TryGetValue(HeaderName, out var key);

            if (!hasKey || key != ExpectedKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(
                    "Unauthorized: send header 'X-API-KEY: dev-secret' to use this endpoint.");
                return;
            }

            await _next(context);
        }
    }
}
