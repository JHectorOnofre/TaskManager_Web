/*  De la Arquitectura Modular

* Actualmente: Se hacen 3 cosas en un mismo bloque: buscar un Id, llamar a una URL y mostrar el resultado

* Distribución: 
    - core.js       ->  URLs 
    - api.js        ->  $ajax 
    - service.js    ->  lógicas y manipulación del DOM    
    - utilities.js  ->  
    - events.js     ->  



*/


ocument.addEventListener("DOMContentLoaded", () => {


    const modal = new bootstrap.Modal(document.getElementById("taskModal"));
    const modalContent = document.getElementById("taskModalContent");

    // agg filtros de búsqueda 

    // CREAR
    document.getElementById("btnCrearTask")
        .addEventListener("click", async () => {

            modalContent.innerHTML = spinnerHtml();

            const response = await fetch("/Tasks/CreatePartial");
            console.log(response);
            const html = await response.text();

            modalContent.innerHTML = html;

            await loadCategoriesInModal(modalContent);
            modal.show();
        });


    // EDITAR
    document.addEventListener("click", async (e) => {
        if (e.target.matches(".btnEdit")) {

            const id = e.target.dataset.id;

            modalContent.innerHTML = spinnerHtml();

            const response = await fetch('/Tasks/EditPartial/' + id); //url = core
            const html = await response.text();

            modalContent.innerHTML = html;
            console.log("Hola");
            
            await loadCategoriesInModal(modalContent);
            modal.show();
        }
    });


    // GUARDAR
    document.addEventListener("click", async (e) => {
        if (e.target.id === "btnSaveTask") {

            const form = document.getElementById("taskForm");
            const formData = new FormData(form);

            const data =
                {
                Id: parseInt(formData.get("Id")) || 0,
                Title: formData.get("Title"),
                CategoryId: parseInt(formData.get("CategoryId")) || 0,
                Step: parseInt(formData.get("Step")) || 0,
                //IsCompleted: formData.get("IsCompleted") === "true"
                IsCompleted: form.querySelector('input[name="IsCompleted"][type="checkbox"]').checked
                }

            const isEdit = data.Id && data.Id !== "0";

            const url = isEdit
                ? '/Tasks/Edit/' + data.Id
                    : '/Tasks/Create';

            const response = await fetch(url,
                {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
                });

            if (!response.ok)
                {
                const html = await response.text();
                modalContent.innerHTML = html;
                return;
                }

            modal.hide();

            await refreshTable();
        }
    });


    //ELIMINAR 
    document.addEventListener("click", async (e) => {
        if (e.target.matches(".btnDelete")) {

            const id = e.target.dataset.id;
            if (!id) return;

            const confirmado = confirm("¿Seguro que deseas eliminar esta tarea?");
            if (!confirmado) return;

            try {
                const response = await fetch('/Tasks/DeleteAjax/' + id, {
                    method: "POST"
                });

                const result = await response.json();

                if (!response.ok || !result.success) {
                    showError(result.message || "No se pudo eliminar la tarea.");
                    return;
                }

                showSuccess(result.message || "La tarea fue eliminada correctamente.");

                await refreshTable();

            } catch (err) {
                console.error(err);
                showError("Error de comunicación con el servidor al eliminar la tarea.");
            }
        }
    });



});

function spinnerHtml() {
    return `
        <div class="modal-body text-center">
            <div class="spinner-border text-primary"></div>
            <p>Cargando...</p>
        </div>`;
}


async function refreshTable() {
    const response = await fetch("/Tasks/LoadTablePartial"); //core
    const html = await response.text();
    document.getElementById("taskTableContainer").innerHTML = html;
}


async function loadCategoriesInModal(modalContent) {
    console.log("Hola");
    const select = modalContent.querySelector("#categorySelect");
    if (!select) return;

    const selectedId = select.dataset.selectedCategoryId || "";
    console.log(select.dataset);
    try {
        const response = await fetch("/Categories/Options");

        if (!response.ok) {
            console.error("Error al cargar categorías");
            return;
        }

        const categories = await response.json();

        const firstOption = select.querySelector("option[value='']");
        select.innerHTML = "";
        if (firstOption) {
            select.appendChild(firstOption);
        } else {
            const defaultOpt = document.createElement("option");
            defaultOpt.value = "";
            defaultOpt.textContent = "-- Seleccione una categoría --";
            select.appendChild(defaultOpt);
        }

        categories.forEach(cat => {
            const opt = document.createElement("option");
            opt.value = cat.id;
            opt.textContent = cat.name;

            if (selectedId && selectedId === String(cat.id)) {
                opt.selected = true;
            }

            select.appendChild(opt);
        });

    } catch (err) {
        console.error("Error de red al cargar categorías", err);
    }
}


function showSuccess(message) {
    const container = document.getElementById("alertContainer");
    if (!container) return;

    container.innerHTML = `
       <div class="alert alert-success alert-dismissible fade show" role="alert">
           ${message}
           <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
       </div>`;
}


//function showSuccesstest(message) { // 30 marzo: Ej. de Modularidad reutilizando función de core.js
    
//    if (!ns.core.$alert) return;

//    ns.core.$alert.innerHTML = `
//       <div class="alert alert-success alert-dismissible fade show" role="alert">
//           ${message}
//           <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
//       </div>`;
//}


function showError(message) {
    const container = document.getElementById("alertContainer");
    if (!container) return;

    container.innerHTML = `
       <div class="alert alert-danger alert-dismissible fade show" role="alert">
           ${message}
           <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
       </div>`;
}


