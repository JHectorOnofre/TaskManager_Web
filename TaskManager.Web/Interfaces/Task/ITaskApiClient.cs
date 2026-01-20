using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public interface ITaskApiClient
    {
        Task<PagedResultViewModel<TaskViewModel>> GetTasksAsync(int page = 1, int pageSize = 10);
    }
}