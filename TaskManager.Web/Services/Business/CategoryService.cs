using Microsoft.AspNetCore.Http;

namespace TaskManager.Web.Services.Business
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryService(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<string> ImportFromExcelAsync(IFormFile file)
        {
            // Centra la validación básica de protección
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Debe seleccionar un archivo Excel.");
            }

            // Llama al cliente de API y retorna el mensaje procesado
            return await _categoryApiClient.ImportCategoriesFromExcelAsync(file);
        }

        public async Task<IEnumerable<object>> GetCategoriesAsync()
        {
            // Queda pendiente para otra clase
            return await _categoryApiClient.GetAllCategoriesAsync();
        }
    }
}