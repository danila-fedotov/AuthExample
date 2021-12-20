using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = new TimeSpan(0, 10, 0);
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;

                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorizationCore();

            services.AddTransient<IDbRepository, DbRepository>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
