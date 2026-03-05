using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    // INTERFAZ = contrato (igual que en la API)
    public interface ITaskApiClient
    {
        Task<PagedResultViewModel<TaskViewModel>> GetTasksAsync(int page = 1, int pageSize = 10); //cambia la vista

        Task<PagedResultViewModel<TaskViewModel>> SearchTasksAsync(TaskSearchViewModel filters); //21ene buscar tareas con filtro

        Task<bool> CreateTaskAsync(CreateTaskViewModel model);

        Task<EditTaskViewModel> GetTaskByIdAsync(int id); //23
        Task UpdateTaskAsync(EditTaskViewModel model);

        Task<bool> DeleteTaskAsync(int id);//28 ene

        Task<TaskViewModel> GetTaskDetailAsync (int id);//30

        Task<PagedResultViewModel<TaskViewModel>> AdvancedSearchAsync(TaskSearchViewModel filters); //4feb (index2) & nuevo Index
    }
}
