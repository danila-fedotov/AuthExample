using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.Fias
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFias(this IServiceCollection services)
        {
            services.AddTransient<IFiasService, FiasService>();

            return services;
        }
    }
}
