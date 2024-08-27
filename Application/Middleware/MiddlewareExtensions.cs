using Microsoft.AspNetCore.Builder;

namespace Application.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewareHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>()
                .UseMiddleware<UserStatusMiddleware>();
        }
    }
}