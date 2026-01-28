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
            var response = await _httpClient.PostAsJsonAsync( //manda petición POST con objeto JSTON al endpoint que se indica
                "/api/tasks",
                model
            );

            response.EnsureSuccessStatusCode(); // lanza una excepción si no es exitosa
            return true; // mientras no se controlan excepciones
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
    }
}

