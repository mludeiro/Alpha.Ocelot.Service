using System.Diagnostics;

namespace Ocelot.OpenTelemetry;

public class RequestIdMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if( !context.Response.Headers.ContainsKey("requestId") )
        {
            var requestId = Activity.Current?.TraceId.ToString();
            context.Response.Headers.Append("requestId", requestId ?? string.Empty);
        }

        await next(context);
    }
}
