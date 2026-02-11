using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly HttpClient _httpClient;

        public CategoryApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<string> ImportCategoriesFromExcelAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent(); // emular un formulario HTML con 

            // Convertimos el IFormFile en StreamContent
            using var stream = file.OpenReadStream(); // crea el archivo virtual (emulando en memoria)
            var fileContent = new StreamContent(stream); // acceso para poder manipular el archivo

            // Tipo MIME típico para Excel .xlsx (no es obligatorio pero está bien ponerlo)
            fileContent.Headers.ContentType = 
                new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // El "file" aquí debe coincidir con el nombre del parámetro en el endpoint de la API
            content.Add(fileContent, "file", file.FileName); 

            // Llamamos a la API
            var response = await _httpClient.PostAsync("/api/categories/import-excel", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al importar categorías. Respuesta API: {errorBody}");
            }

            // Leemos el JSON que envía la API
            var result = await response.Content.ReadFromJsonAsync<ImportCategoriesResult>();

            // Si por alguna razón no se pudo deserializar
            if (result == null || string.IsNullOrWhiteSpace(result.Message))
            {
                return "Importación realizada correctamente.";
            }

            // Devolvemos el mensaje que vino de la API
            // Ej: "Se importaron 5 categorías nuevas."
            return result.Message +
                   (result.Duplicadas > 0
                        ? $" ({result.Duplicadas} filas duplicadas no se importaron.)"
                        : string.Empty);
        }

        // 2: Se modifica el servicio con la URL que apunte a lo que se hizo en la API (a la UrL)
        //public async task<importcategoriesresult> importcatalogoasync(stream filestream, string filename)
        //{
        //    using var content = new multipartformdatacontent();
        //    var streamcontent = new streamcontent(filestream);
        //    content.add(streamcontent, "file", filename);

        //    var response = await _httpclient.postasync("api/categories/import-excel", content); //url que iene del endpoint de la api

        //    if (!response.issuccessstatuscode)
        //    {
        //        var error = await response.content.readasstringasync();
        //        throw new exception(error);
        //    }

        //    return await response.content.readfromjsonasync<importcategoriesresult>();
        //}
        public async Task<ImportCategoriesResult> ImportFromExcelAsync(Stream fileStream, string fileName)
        {
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);

            // "file" coincide con el parámetro IFormFile file del controlador API
            content.Add(streamContent, "file", fileName);

            // La URL que confirmamos en tu CategoriesController de la API
            var response = await _httpClient.PostAsync("api/categories/import-excel", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return await response.Content.ReadFromJsonAsync<ImportCategoriesResult>();
        }
    }
}
