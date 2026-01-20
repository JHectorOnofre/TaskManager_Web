using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Services;

namespace TaskManager.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskApiClient _client;

        public TasksController(ITaskApiClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _client.GetTasksAsync(page, pageSize);
            return View(result);
        }
    }
}
