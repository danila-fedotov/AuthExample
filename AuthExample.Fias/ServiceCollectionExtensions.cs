using Microsoft.Extensions.DependencyInjection;

namespace AuthExample.Fias
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
