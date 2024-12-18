using CristmassTree.Services.Contracts;

namespace CristmassTree.Presentation.Middleware;

public class TokenCaptureMiddleware
{
    private readonly RequestDelegate next;

    public TokenCaptureMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = context.RequestServices.CreateScope())
        {
            var tokenTracker = scope.ServiceProvider.GetRequiredService<ITokenTracker>();

            string token = string.Empty;

            if (context.Request.Headers.ContainsKey("Christmas-Token"))
            {
                token = context.Request.Headers["Christmas-Token"]!;
            }

            if (!string.IsNullOrEmpty(token))
            {
                await tokenTracker.TrackTokenAsync(token);
            }

            await next(context);
        }
    }
}