namespace TaskManager.Web.Models
{

    /*Los Modelos NO son la BD, NO son entidades, sino la forma en que 
     MVC entiende una respuesta de la API
    
     - Los Modelos se crean para deserializar el JSON (lo que devuelve la API), 
        si se quiere usar ese JSON en la API lo convertimos en un objeto (deserializar)
     */
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int Step { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
