namespace TaskManager.Web.Models
{
    /* = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
        Este modelo sirve para representar lo que el usuario pone en la pantalla
        - Los filtros
        - los resultados que el API mande
        - Guarda tanto los filtros (texto, categoría, etc), como el resultado de la búsqueda (Result)
        * usado originalmente para el ejemplo del Index2.cshtml (Guía-día3 / sesión 4feb)
     = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =  */

    public class TaskSearchViewModel
    {
        // Filtros del usuario
        public string? Text { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsCompleted { get; set; }
        public int? Step { get; set; }


        // Datos de paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5; // Paginación de 10 a 5 en la ruta /Task/Search 22ene

        public int? CategoryId { get; set; } // 4feb


        // Resultados devueltos por la API
        public PagedResultViewModel<TaskViewModel>? Result { get; set; }
    }
}



/* De los ViewModel:
 * 
 * DTO = Mensajero
 * ViewModel = modelo de vista (para representar los datos como lo necesite la UI
 * 
 - A diferencia del DTO en un ViewModel es común tener mezclados los parámetros
    de entrada y su respuesta (L16)
 
 - Los ViewModel viven en la solución Frontend

 - DTO != ViewModel: Se trata de capas separadas, por ende NO comparten tipos
 
 */