using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Interfaces.Business;

namespace TaskManager.Web.Controllers
{
    public class CategoriesController : Controller
    {
        // Ahora inyectamos el SERVICIO en lugar del CLIENTE
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                // El controlador solo delega la tarea al servicio
                var resultMessage = await _categoryService.ImportFromExcelAsync(file);
                TempData["Success"] = resultMessage;
            }
            catch (ArgumentException ex)
            {
                // Errores de validación conocidos
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                // Errores inesperados de la API
                TempData["Error"] = "Ocurrió un error al importar el archivo: " + ex.Message;
            }

            return View();
        }

        public IActionResult Index()
        {
            return View(); // Queda pendiente para otra clase
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoriesJson()
        {
            // Supongamos que tu cliente de categorías tiene un método para listar
            var categories = await _categoryService.GetCategoriesAsync();
            return Json(categories);
        }

        //250226
        // GET: /Categories/Options
        [HttpGet]
        public async Task<IActionResult> Options()
        {
            var categories = await _categoryService.GetSimpleListAsync();
            return Json(categories); // Devuelve JSON al JS del front
        }
    }
}