using System.Net;

namespace ShopIndexWebApp.Server;

public class NotFoundImageMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundImageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            if (!context.Request.Path.StartsWithSegments("/assets/items"))
                return;

            context.Response.Redirect("/assets/items/minecraft/air.png");
        }
    }
}

public static class NotFoundImageMiddlewareExtensions
{
    public static IApplicationBuilder UseNotFoundImageMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NotFoundImageMiddleware>();
    }
}
