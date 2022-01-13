using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddSingleton<ISessionService, SessionService>();
            services.AddTransient<IDbRepository, DbRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();

            return services;
        }
    }
}
