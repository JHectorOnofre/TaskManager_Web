using TaskManager.Web.Services;
using TaskManager.Web.Services.Business;

namespace TaskManager.Web.Extensions
{
    public static class ServiceCollectionExtensions  //Inyeccion de dependecias sobre est metodo par ano tocar el Program.cs
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services)
        {
            // 1. Clientes de API (Capa de Infraestructura)
            services.AddHttpClient<ITaskApiClient, TaskApiClient>();
            services.AddHttpClient<ICategoryApiClient, CategoryApiClient>();
            // 2. Servicios de Negocio (Service Layer)
            // Se usa AddScoped para que el servicio viva lo que dura la petición HTTP
            services.AddScoped<ITaskService, TaskService>(); //IMplementacióin de Service Layer
            services.AddScoped<ICategoryService, CategoryService>(); //IMplementacióin de Service Layer

            return services;
        }
    }
}