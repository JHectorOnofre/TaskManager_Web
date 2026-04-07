/*
Service 'Service Layer' = Lógica 

- Llamado a los métodos que contienen la lógica (Capa que orquesta el flujo)
    + Es decir: toma datos de entrada, toma valores, interpreta respuestas, actualiza estados
        puede deolver estados claros a eventos particulares
- No per sé la lógica de negocio, pero sí la lógica del front

- Se encarga de cuándo hacer el llamado y  cómo hacer el flujo de los datos
- Ayuda a validar cuestiones
*/




/* con base a jQuery
* - manejo de escenarios del lado del service cuando el API no responde como se espera
* - servive es quien maneja las categorías 
* 
*/
(function (ns) {
    //const ns = window.Tasks.index = window.Tasks.index || {};
    ns.service = ns.service || {};


    ns.service.refreshTable = async function () {
        try {
            const result = await ns.api.refreshTable();
            if (result) {
                ns.core.selectors.$taskTable.html(result);
            }
        } catch (error) {
            console.error("Error al refrescar la tabla", error);
        }
    };


    // la carga de categorías en el modal
    ns.service.loadCategoriesInModal = async function () {
        const $element = ns.core.selectors.$modalContent.find("#categorySelect");
        if (!$element.length) return;

        const selectedId = $element.data("selectedCategoryId");

        try {
            const categories = await ns.api.getCategoryOptions();

            $element.empty().append('<option value="">-- Seleccione --</option>');

            categories.forEach(cat => {
                // "==" para comparar string con number si es necesario
                const isSelected = selectedId == cat.id ? "selected" : "";
                $element.append(`<option value="${cat.id}" ${isSelected}>${cat.name}</option>`);
            });
        } catch (error) {
            console.error("Error al cargar categorías", error);
        }
    };


    ns.service.saveTask = async function () {
        const formData = new FormData(ns.core.selectos.$taskForm);
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

        try {
            const response = await ns.api.saveTask(data, isEdit);

            if (response.ok) {
                ns.core.selectors.$taskModal.hide();
                await ns.service.refreshTable();
            } else {
                const html = await response.text();
                ns.core.selectors.taskModalContent.html(html);
                return;
            }

        }
        catch (error) {
            console.error("Error en el proceso de guardado", error);
        }
    };

})(window.Tasks.index);


//(function (ns) {
//    ns.service = ns.service || {};

//    ns.service.refreshTable = async function() {

//        let result = await ns.api.refreshTable();

//        if (result !== null) {
//            ns.core.selectors.$refreshTable.html(result);
//        }
//    };


//    ns.service.loadCategoriesInModal = async function () {

//        let element = ns.core.selectors.$modalContent.find("#categorySelect");

//        if (!$element.length) {
//            return;
//        }

//        let id = element.attr("id");
//    }

//});
