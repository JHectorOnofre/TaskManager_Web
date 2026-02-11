using Microsoft.AspNetCore.Http;
using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public interface ICategoryApiClient
    {
        Task<string> ImportCategoriesFromExcelAsync(IFormFile file); // para POST

        // 1. "Contrato" para "Catalogo_categorias" / Asignación Excel - Front que coincide con el de la API
        Task<ImportCategoriesResult> ImportFromExcelAsync(Stream fileStream, string fileName);
    }
}