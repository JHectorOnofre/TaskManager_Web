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

        public async Task<IActionResult> Index(TaskSearchViewModel filters) // NUEVO Index (adaptando lo visto en index2 - 4feb)
        {
            
            if (filters.Page <= 0) filters.Page = 1; //Valida que la página sea > 0

            filters.PageSize = 5; // para que siempre muestre 5 registros

            var result = await _client.AdvancedSearchAsync(filters); // llamada al servicio de búsqueda avanzada

            filters.Result = result; // inyección de los resultados dentro del mismo modelo de filtros

            return View(filters); // devolver la vista con el modelo completado (filtros + resultado)
        }


        // INDEX 1 = /Tasks/Search
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


        [HttpGet] // 4feb
        /* Se añade lógice en un "index2" para no crear conflictos en el Index principal
         *  
            -> Proviene de la comunicación con el servicie "TaskApiClient"
            -> se comunica con la interfaz "ITaskApiClient"
            -> Se crea un index2.cshtml < Tasks < Views

            * Consiste en retornar el mismo viewModel que recibimos, únicamente le asignamos sobre la propiedada "result" lo que la API haya respondido
            (Mantener lo sdatos en el ciclo para que el usuario pueda verlos y no se pierda)
         */
        public async Task<IActionResult> Index2(TaskSearchViewModel filters)
        {
            var result = await _client.AdvancedSearchAsync(filters);
            filters.Result = result;
            return View(filters); // regresamos siempre el modelo completo
        }


        [HttpGet] // 5 feb
        public IActionResult AjaxDemo()
        {
            return View();
        }


        [HttpGet] // 12 feb
        public async Task<IActionResult> LoadTablePartial(TaskSearchViewModel filters)
        {
            var result = await _client.AdvancedSearchAsync(filters);
            return PartialView("_TaskTablePartial", result.Items);
        }



    }
}
