using TaskManager.Web.Services;

namespace TaskManager.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Método de Extensión para Inyección de Dependencias
        public static IServiceCollection AddApiClients(this IServiceCollection services)
        {
            services.AddHttpClient<ITaskApiClient, TaskApiClient>();

            services.AddHttpClient<ICategoryApiClient, CategoryApiClient>();

            return services;
        }
    }
}
