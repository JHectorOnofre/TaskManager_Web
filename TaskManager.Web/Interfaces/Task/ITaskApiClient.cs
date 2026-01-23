using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public interface ITaskApiClient
    {
        Task<PagedResultViewModel<TaskViewModel>> GetTasksAsync(int page = 1, int pageSize = 10);

        Task<PagedResultViewModel<TaskViewModel>> SearchTasksAsync(TaskSearchViewModel filters); //21ene buscar tareas con filtro

        Task<bool> CreateTaskAsync(CreateTaskViewModel model); 
    }
}
