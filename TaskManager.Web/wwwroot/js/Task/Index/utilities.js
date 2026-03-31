/*
Utilities = 
- funciones de apoyo, p. ej. fromateo de fechas , convertir valores,...
- funciones pequeñas, claras y legibles; reutilizables, independientes (no acopladas a otra cosa ni del DOM), fáciles de probar

"¿esta función tendría sentido fuera de un flujo específico?" <= Útil = versatil

- registros, handlers, conexión interfaz usuario-servicios, 
- llamados al servicio, mostrar msj si aplica, abrir y cerrar modales (puntuales), refrescar secciones visuales,
    
- Delega lógica pesada a otra capa (p. ej. el Service)
- NO Ajax directos : Peticiones por medio del api, no una api directamente y seguir un flujo similar al servicio
-  
 

Evento != Proceso
- Evento =disparador (acción, p. ej. un click) 
- proceso = lo que se ejecuta a partir dedicha interacción, el servicio procesará el resultado de dicha interacción

*/

(function (window, $) {

    const ns = window.Tasks.index = window.Tasks.index || {};
    ns.events = ns.events || {};


    ns.init = function () {
    };
})(window, jQuery);