using TaskManager.Web.Services;

namespace TaskManager.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services)
        {
            services.AddHttpClient<ITaskApiClient, TaskApiClient>();

            return services;
        }
    }
}
