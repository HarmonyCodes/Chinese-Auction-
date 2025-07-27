namespace Server
{
    public class CustoMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public CustoMiddleware(RequestDelegate next, ILogger<CustoMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request Path{context.Request.Path}");
            // Call the next middleware in the pipeline
            await _next(context);
            _logger.LogInformation("CustoMiddleware: Request ended at {Time}", DateTime.UtcNow);
        }
        
    }
}
