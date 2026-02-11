using Microsoft.AspNetCore.Mvc;
using TaskManager.Web.Services;

namespace TaskManager.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoriesController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet] // Para "renderizar" el formulario
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost] // Tomra el archivo dsde la API y mostrar respuesta al usuario
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar un archivo Excel.";
                return View();
            }

            try
            {
                var resultMessage = await _categoryApiClient.ImportCategoriesFromExcelAsync(file);
                TempData["Success"] = resultMessage;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al importar el archivo: " + ex.Message;
            }

            return View();
        }


        public IActionResult Index() // para hacer un index de categorías
        {
            return View(); // Queda pendiente para otra clase
        }



        [HttpPost]
        public async Task<IActionResult> ImportCatalogo(IFormFile file)
        {
            // ... validación de archivo ...

            // Llamamos al ÚNICO método del API Client que existe
            var result = await _categoryApiClient.ImportFromExcelAsync(file.OpenReadStream(), file.FileName);

            TempData["Success"] = result.Message;
            return View();
        }
    }
}
