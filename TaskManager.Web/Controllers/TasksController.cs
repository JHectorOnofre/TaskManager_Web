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

        //public async Task<IActionResult> Index(int page = 1, int pageSize = 10) // https:// localhost:7137/
        //{
        //    var result = await _client.GetTasksAsync(page, pageSize);
        //    return View(result);
        //}
        // - - Se sustituye por la unificación de Index + Index2 (asignación feb 04):
        public async Task<IActionResult> Index(TaskSearchViewModel model)
        {
            // 1. Configuramos valores por defecto si vienen vacíos
            if (model.Page <= 0) model.Page = 1;
            model.PageSize = 5; // se mantiene el estándar de 5 resultados por página

            // 2. El cliente para traer los datos filtrados
            model.Result = await _client.AdvancedSearchAsync(model); // Aquí usamos "AdvancedSearchAsync" que es el que ya tienes funcionando en Index2

            // 3. Retorna la vista (Index principal) con el modelo completo
            return View(model);
        }

        //public async Task<IActionResult> Search(TaskSearchViewModel model)
        //{
        //    // Si es la primera carga de la página
        //    if (model.Page == 0) // antes ==
        //        model.Page = 1;

        //    model.PageSize = 5; // modificar búsqueda a 5 resultados

        //    model.Result = await _client.SearchTasksAsync(model); // se envía el modelo a la API

        //    return View("Index1", model);
        //} - - LÓGICA QUE ANTES REDIRECCIONABA A INDEX1, AHORA INDEX PUEDE FILGRAR
        

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


        [HttpGet] //23ene
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _client.GetTaskByIdAsync(id);
            return View(model); // resultado de la vista
        }


        [HttpPost] //23ene
        public async Task<IActionResult> Edit(EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _client.UpdateTaskAsync(model);
                TempData["Success"] = "La tarea fue actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
                return View(model); // ya viene con el mensaje de error
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _client.DeleteTaskAsync(id);
                TempData["Success"] = "La tarea fue eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo eliminar la tarea: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet] // 30ene: 
        public async Task<IActionResult> Details(int id)
        {
            var task = await _client.GetTaskDetailAsync(id);

            if (task == null)
            {
                TempData["Error"] = "La tarea no existe.";
                return RedirectToAction("Index");
            }

            return View(task);
        }


        //[HttpGet] // 4feb
        //public async Task<IActionResult> Index2(TaskSearchViewModel filters)
        //{
        //    var result = await _client.AdvancedSearchAsync(filters);
        //    filters.Result = result;
        //    return View(filters); // regresamos siempre el modelo completo
        //} - - Sustituido para unificar con Index


        [HttpGet]
        public IActionResult AjaxDemo()
        {
            return View();
        }
    }
}
