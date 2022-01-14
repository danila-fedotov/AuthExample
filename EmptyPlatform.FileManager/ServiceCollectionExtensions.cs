using EmptyPlatform.FileManager.Db;
using EmptyPlatform.FileManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.FileManager
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManager(this IServiceCollection services, FileManagerConfiguration configuration)
        {
            services.AddSingleton(sp => configuration);
            services.AddTransient<IDbRepository, DbRepository>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
