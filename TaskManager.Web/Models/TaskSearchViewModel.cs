namespace TaskManager.Web.Models
{
    // modelo que representa lo que el usuario escribe
    public class TaskSearchViewModel
    {
        // Filtros del usuario
        public string? Text { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsCompleted { get; set; }
        public int? Step { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Resultados devueltos por la API
        public PagedResultViewModel<TaskViewModel>? Result { get; set; }
    }
}
/* A diferencia del DTO en un ViewModel es común tener mezclados los parámetros
 de entrada y su respuesta (L16) */