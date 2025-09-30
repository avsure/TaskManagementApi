namespace TaskManagementApi.Middleware
{
    public class SerilogCorrelationEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogCorrelationEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Items["X-Correlation-ID"]?.ToString();

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId ?? string.Empty))
            {
                await _next(context);
            }
        }
    }
}
