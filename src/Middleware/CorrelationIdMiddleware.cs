namespace TaskManagementApi.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationHeader] = correlationId;
            }

            context.Response.Headers[CorrelationHeader] = correlationId;

            // Store in HttpContext for Serilog
            context.Items[CorrelationHeader] = correlationId;

            await _next(context);
        }
    }
}
