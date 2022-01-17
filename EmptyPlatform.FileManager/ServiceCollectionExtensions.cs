using EmptyPlatform.FileManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.FileManager
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManager(this IServiceCollection services, FileManagerConfiguration config)
        {
            services.AddSingleton(config);
            services.AddScoped<IDbRepository, DbRepository>();
            services.AddTransient<IFileStorage, FileStorage>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
