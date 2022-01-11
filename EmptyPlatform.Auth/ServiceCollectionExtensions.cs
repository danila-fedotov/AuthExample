using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            //var aa = Assembly
            //   .GetEntryAssembly()
            //   .GetReferencedAssemblies()
            //   .Select(Assembly.Load)
            //   .SelectMany(x => x.DefinedTypes)
            //   .SelectMany(x => x.GetMembers());
            //var a = aa
            //   .Where(x => x.GetCustomAttributes<AuthorizeAttribute>().Any());

            services.AddSingleton<ISessionService, SessionService>();
            services.AddScoped<IDbRepository, DbRepository>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
