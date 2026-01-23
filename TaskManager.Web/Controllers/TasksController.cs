using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Models;
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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10) // https:// localhost:7137/
        {
            var result = await _client.GetTasksAsync(page, pageSize);
            return View(result);
        }


        public async Task<IActionResult> Search(TaskSearchViewModel model)
        {
            // Si es la primera carga de la página
            if (model.Page == 0) // antes ==
                model.Page = 1;

            model.PageSize = 5; // modificar búsqueda a 5 resultados

            model.Result = await _client.SearchTasksAsync(model); // se envía el modelo a la API

            return View("Index1", model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateTaskViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _client.CreateTaskAsync(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
