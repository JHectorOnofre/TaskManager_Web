using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Models;
using TaskManager.Web.Services.Business; // Referencia a la nueva capa

namespace TaskManager.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Arreglo DCC improvisado 230126
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var model = await _taskService.GetIndexModelAsync(page, pageSize);
            return View(model);
        }

        public async Task<IActionResult> Search(TaskSearchViewModel model)
        {
            var resultModel = await _taskService.SearchTasksAsync(model);
            return View("Index1", resultModel);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateTaskViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _taskService.CreateTaskAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        // El [FromBody] es OBLIGATORIO si fetch envía application/json
        public async Task<IActionResult> Edit([FromBody] EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Para que el AJAX sepa que hubo un error

            try
            {
                await _taskService.UpdateTaskAsync(model);
                return Ok(); // Responde OK(200) para que el JavaScript cierre el modal y actualice la tabla
            }
            catch (Exception ex)
            {
                return BadRequest("Ocurrió un error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {//Borramo try catch por modificaciones en el MIddleware, permitiendo capturar excepciones 130226
            try
            {
                await _taskService.DeleteTaskAsync(id);
                TempData["Success"] = "La tarea fue eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo eliminar la tarea: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // Versiones con mensaje UX por falta de ID
        public async Task<IActionResult> Details(int? id)
        {
            var task = await _taskService.GetDetailsAsync(id);
            if (task == null)
            {
                TempData["Error"] = (id == null || id == 0)
                    ? "Por favor indica el registro que quieres ver detalles."
                    : "La tarea no existe.";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var task = await _taskService.GetTaskForEditAsync(id);
            if (task == null)
            {
                TempData["Error"] = "Por favor indica el registro que quieres editar.";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // 060326 Import Excel tareas
        [HttpGet]
        public IActionResult Import() => View();

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar un archivo Excel.";
                return View();
            }
            try
            {
                TempData["Success"] = await _taskService.ImportFromExcelAsync(file);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al importar el archivo: " + ex.Message;
            }
            return View();
        }

        // 040226 Advanced Search
        [HttpGet]
        public async Task<IActionResult> Index2(TaskSearchViewModel filters)
        {
            var model = await _taskService.GetAdvancedSearchModelAsync(filters);
            return View(model);
        }

        // 050226 Ajax
        [HttpGet]
        public IActionResult AjaxDemo() => View();

        //120226 Partial Views GAdaptsacion
        [HttpGet]
        public async Task<IActionResult> LoadTablePartial(TaskSearchViewModel filters)
        {
            // Cambia _client por _taskService y usa el método correspondiente
            var resultModel = await _taskService.AdvancedSearchAsync(filters);

            // Como AdvancedSearchAsync devuelve el ViewModel completo, 
            // pasamos la lista de items que está dentro de .Result
            return PartialView("_TaskTablePartial", resultModel.Result?.Items);
        }
        //180226
        [HttpGet]
        public IActionResult CreatePartial()
        {
            // El TRUCO: Pasamos un EditTaskViewModel con Id = 0 para que la vista no colapse al buscar el Model.Id
            return PartialView("_TaskFormPartial2", new EditTaskViewModel { Id = 0 });
        }

        // 2. GET: Para abrir el modal de EDITAR
        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            return PartialView("_TaskFormPartial2", task);
        }

    }
}