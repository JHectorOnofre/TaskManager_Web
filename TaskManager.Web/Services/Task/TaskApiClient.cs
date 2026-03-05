using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TaskManager.Web.Models;

namespace TaskManager.Web.Services
{
    public class TaskApiClient : ITaskApiClient
    {
        private readonly HttpClient _httpClient;

        public TaskApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<PagedResultViewModel<TaskViewModel>> GetTasksAsync(int page = 1, int pageSize = 10)
        {
            var url = $"/api/tasks/advanced-search?page={page}&pageSize={pageSize}";

            var result = await _httpClient.GetFromJsonAsync<PagedResultViewModel<TaskViewModel>>(url);

            return result!;
        }

        public async Task<PagedResultViewModel<TaskViewModel>> SearchTasksAsync(TaskSearchViewModel filters) // 21 ene
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(filters.Text))
                query.Add($"text={filters.Text}");

            if (!string.IsNullOrWhiteSpace(filters.CategoryName))
                query.Add($"categoryName={filters.CategoryName}");

            if (filters.IsCompleted.HasValue)
                query.Add($"isCompleted={filters.IsCompleted.Value}");

            if (filters.Step.HasValue)
                query.Add($"step={filters.Step.Value}");

            query.Add($"page={filters.Page}");
            query.Add($"pageSize={filters.PageSize}");

            var finalQueryString = string.Join("&", query);

            var url = $"/api/tasks/advanced-search?{finalQueryString}";

            return await _httpClient.GetFromJsonAsync<PagedResultViewModel<TaskViewModel>>(url);
        }

        public async Task<bool> CreateTaskAsync(CreateTaskViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync( //manda petición POST con objeto JSON al endpoint que se indica
                "/api/tasks",
                model
            );

            response.EnsureSuccessStatusCode(); // lanza una excepción si no es exitosa
            return true; // mientras no se controlan excepciones
        }
        public async Task<TaskViewModel> GetTaskDetailAsync(int id) //30 ene
        {
            return await _httpClient.GetFromJsonAsync<TaskViewModel>($"/api/tasks/{id}");
        }

         
        public async Task<EditTaskViewModel> GetTaskByIdAsync(int id) //23 ene
        {
            var response = await _httpClient.GetFromJsonAsync<TaskViewModel>($"/api/tasks/{id}");

            return new EditTaskViewModel
            {
                Id = response.Id,
                Title = response.Title,
                CategoryId = response.CategoryId,
                Step = response.Step,
                IsCompleted = response.IsCompleted
            };
        }

        public async Task UpdateTaskAsync(EditTaskViewModel model) //23 ene
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"/api/tasks/{model.Id}",
                model
            );

            if (!response.IsSuccessStatusCode) // se verifica 
            {
                // Leer mensaje de la API
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/tasks/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return true;
        }

        public async Task<PagedResultViewModel<TaskViewModel>> AdvancedSearchAsync(TaskSearchViewModel filters) //4feb
            /* retorna el resultado esperado por el PagedResultViewModel (el que trae la lista de registros, los filtros aplicados y la página donde se está posicionado 
                - dentro de éste está el otro modelo TaskViewModel para traer la lista de registros que coincidan con los criterios de búsqueda
                - el método llamado "AdvancedSearchAsync" recibe un ViewModel de entrada llamado "TaskSearchViewModel" 
            */
        {
            var query = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(filters.Text))
                query["text"] = filters.Text; //asignación directa

            if (!string.IsNullOrWhiteSpace(filters.CategoryName))
                query["categoryName"] = filters.CategoryName;

            if (filters.CategoryId.HasValue)
                query["categoryId"] = filters.CategoryId.Value.ToString();//conversión por tipado requerido

            if (filters.Step.HasValue)
                query["step"] = filters.Step.Value.ToString();

            if (filters.IsCompleted.HasValue)
                query["isCompleted"] = filters.IsCompleted.Value.ToString().ToLower();

            query["page"] = filters.Page.ToString();
            query["pageSize"] = filters.PageSize.ToString();

            // Construir una URL con QueryString dinámico
            var queryString = string.Join("&",
                query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            var url = $"/api/tasks/advanced-search?{queryString}";

            return await _httpClient.GetFromJsonAsync<PagedResultViewModel<TaskViewModel>>(url)
                   ?? new PagedResultViewModel<TaskViewModel> // ?? ops de coalescencia nula: asignar un valor predeterminado si es null
                   {
                       Items = new List<TaskViewModel>(),
                       Page = filters.Page,
                       PageSize = filters.PageSize,
                       TotalCount = 0
                   };
        }
    }


}

