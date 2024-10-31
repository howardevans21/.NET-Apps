namespace DWPipelineWebService.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");

                return;
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

            //    var apiKey = appSettings.GetValue<string>(APIKEY);
            var section = appSettings.GetSection("Settings");
            var apiKeySection = section.GetSection(APIKEY);
            var apiKey = apiKeySection.Value;

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }
            await _next(context);
        }
    }
}
