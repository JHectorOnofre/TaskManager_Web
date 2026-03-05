/* Sesión del 5 de febreo / día 8 de la Guía
    - JS puro 
    - 
*/ 

document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById("searchAjax");
    const tableBody = document.getElementById("ajaxResults");

    input.addEventListener("input", async () => {
        const text = input.value;

        /* AJUSTE al error final de la sesión feb 5:
        // const url = `/api/tasks/ajax-search?text=${encodeURIComponent(text)}`;
         
        Este archivo no puede usar una ruta relativa (/api/...) porque el navegador interpreta que la API 
        está en el mismo puerto que el MVC, para este caso:
        - API = 7109
        - MVC = 7137
        */
        const url = `https://localhost:7109/api/tasks/ajax-search?text=${encodeURIComponent(text)}`;
        
        try {
            const response = await fetch(url);
            if (!response.ok) throw new Error("Error en la API");

            const data = await response.json();

            // Limpiar tabla
            tableBody.innerHTML = "";

            // Insertar filas
            data.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.title}</td>
                        <td>${item.categoryName}</td>
                        <td>${item.step}</td>
                        <td>${item.isCompleted ? "Sí" : "No"}</td>
                        <td>
                            <a href="/Tasks/Details/${item.id}" class="btn btn-sm btn-info">Ver</a>
                        </td>
                    </tr>
                `;
                tableBody.innerHTML += row;
            });

        } catch (err) {
            console.error("Error:", err);
        }
    });
});
