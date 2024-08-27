using Application.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Middleware
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userClaims = context.User.Claims;
            var userStatus = userClaims.FirstOrDefault(c => c.Type == ClaimConstant.ActivityStatus)?.Value;

            if (userStatus == nameof(ActivityStatus.Inactive))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Account is inactive.");
                return;
            }

            await _next(context);
        }
    }

}
