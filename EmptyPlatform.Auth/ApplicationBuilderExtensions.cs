using Microsoft.AspNetCore.Builder;

namespace EmptyPlatform.Auth
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
