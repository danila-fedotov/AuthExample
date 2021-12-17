using AuthExample.Auth.Db;
using Microsoft.Extensions.DependencyInjection;

namespace AuthExample.Auth
{
    public class AuthStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbRepository, DbRepository>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
