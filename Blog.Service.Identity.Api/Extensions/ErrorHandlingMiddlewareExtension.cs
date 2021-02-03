using Blog.Service.Identity.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Blog.Service.Identity.Api.Extensions
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
