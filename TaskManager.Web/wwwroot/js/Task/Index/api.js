/*
api = archivo que se encarga de las peticiones (sólo habla con el back)

> funciones que suelen recibir parámetros, llaman al endpoint y devuelven result
> No leen inputs desde el DOM (extraer el elemento directamente del DOM), este debe ser extraído en la capa corresondiente
> No debería mostrar alertas (a priori, según se trbaje)
> No debería incluir reglas de negocio (la gestión va a parte)
> No modifica tablas de la pantalla ni modifica respuestas de la interfaz

Proceso:
- Antes (task-modal.js) uso del Fetch = Se debe escribir manualmente el encabezado (header), convertir el cuerpo a JSON y verificar manualmente la respuesta (ok)
- Ahora (jQuery) = $.ajax

*/


(function (ns) {
    ns.api = ns.api || {};

    // Usamos las URLs definidas en core.js
    const urls = ns.core.urls;


    ns.api.getCreatePartial = function () {
        return $.ajax({
            url: urls.createPartial,
            type: 'GET'
        });
    };

    ns.api.getEditPartial = function (id) {
        return $.ajax({
            url: urls.editPartial + id,
            type: 'GET'
        });
    };

    ns.api.saveTask = function (payload, isEdit) {
        const url = isEdit ? urls.edit + payload.Id : urls.create;
        return $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload)
        });
    };

    ns.api.deleteTask = function (id) {
        return $.ajax({
            url: urls.delete + id,
            type: 'POST'
        });
    };

    ns.api.refreshTable = function () {
        return $.ajax({
            url: urls.loadTable,
            type: 'GET'
        });
    };

    // para las categorías
    ns.api.getCategoryOptions = function () {
        return $.ajax({
            url: urls.categoryOptions,
            type: 'GET'
        });
    };

})(window.Tasks.index);





/*  EJEMPLO REFERENCIA: LOS LLAMADOS SE HARÍAN TAL QUE: ns.api.nombreDelMetodo
    
        ns.validaEstatusTV429 = async function () {
            loading.show();

            ns.datosValidaEstatusTV4 = null;

            model = {
                nClaveEmpresa: ns.contexto.nClaveEmpresa,
                nClaveOficinaTV: ns.$txtnBaseImpresionTVClave.val(),
                nFolioTV: ns.$txtnFolioTV.val(),
                nClaveOficinaLiquidacion: ns.contexto.nClaveOficina,
                nClaveAutobus: ns.nAutobusInicio,
                nKilometros: ns.nAutobusInicio,
                nClaveOperadorUno: ns.nConductor1Inicio,
                nClaveOperadorDos: ns.nConductor2Inicio
            };

            return $.ajax({
                url: ns.urls.validaEstatusTV429Evento,
                type: 'POST',
                data: model
            }).then(async data => {
                if (data.success) {
                    ns.datosValidaEstatusTV4 = data.data;
                    loading.hide();
                    return true;
                } else {
                    notificacion.warning("No se pudo validar el estatus de la TV.");
                }

                loading.hide();
                return false;
            }).catch(async error => {
                notificacion.error("Ocurrió un error al verificar el estatus de la TV.");
                loading.hide();
                return false;
            });
        };



    Se asignan por separado, pero contenidas dentro de api, de manera que sea observable si 
*/




//                                     - - - USANDO JS CLÁSICO - - -
//(function (ns) {                  
//    ns.api = ns.api || {};

//    // Básicos para el Modal
//    ns.api.getCreatePartial = async function () {

//        return await fetch("/Tasks/CreatePartial")
//            .then(async response => {
//                if (!response.ok) throw new Error("Error al obtener formulario de creación");
//                return await response.text();
//            })
//            .catch(error => {
//                console.error(error);
//                return null;
//            });
//    };

//    ns.api.getEditPartial = async function (id) {
//        return await fetch(`/Tasks/EditPartial/${id}`)
//            .then(async response => {
//                if (!response.ok) throw new Error("Error al obtener formulario de edición");
//                return await response.text();
//            })
//            .catch(error => {
//                console.error(error);
//                return null;
//            });
//    };

//    /**
//     * Envía los datos de Crear o Editar).
//     * @param {Object} payload -> Objeto con los datos de la tarea.
//     * @param {Boolean} isEdit -> Indica si es una edición para elegir el endpoint.
//     * @ params => Documentación usada en escenarios en los que se trabaja con módulos 
//     */
//    ns.api.saveTask = async function (payload, isEdit) {
//        const url = isEdit ? `/Tasks/Edit/${payload.Id}` : '/Tasks/Create';

//        return await fetch(url, {
//            method: "POST",
//            headers: { "Content-Type": "application/json" },
//            body: JSON.stringify(payload)
//        }).then(async response => {

//            if (!response.ok) {
//                const errorHtml = await response.text();
//                return { success: false, html: errorHtml };
//            }
//            return { success: true };
//        }).catch(error => {
//            console.error("Error en api.saveTask:", error);
//            return { success: false, message: "Error de comunicación con el servidor." };
//        });
//    };

//    // Eliminar
//    ns.api.deleteTask = async function (id) {
//        return await fetch(`/Tasks/DeleteAjax/${id}`, {
//            method: "POST"
//        }).then(async response => {
//            const result = await response.json();
//            return {
//                success: response.ok && result.success,
//                message: result.message
//            };
//        }).catch(error => {
//            console.error("Error en api.deleteTask:", error);
//            return { success: false, message: "Error de red al intentar eliminar." };
//        });
//    };


//    // Actualización parcial de la tabla
//    ns.api.refreshTable = async function () {
//        return await fetch("/Tasks/LoadTablePartial")
//            .then(async response => {
//                if (!response.ok) return null;
//                return await response.text();
//            });
//    };

//    // Listado de categorias
//    ns.api.getCategoryOptions = async function () {
//        return await fetch("/Categories/Options")
//            .then(async response => {
//                if (!response.ok) throw new Error();
//                return await response.json();
//            })
//            .catch(() => []); // devuelve array vacío si falla la ejecuciión
//    };

//})(window.App.Task); // corresponde al ns global

