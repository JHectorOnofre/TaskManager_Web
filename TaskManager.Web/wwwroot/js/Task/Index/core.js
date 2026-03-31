/*
Core = 
    - Moódulo para definición de variables (namespace, url de módulos...), archivos compartidos, variables de contexto...
    - Selectores comunes son identificados como "ns.selectores" o "ns.$name" 

    !! NO almacena handles, de evento, ajax, lógica de negocio, funciones ni procesos completos (como guardar, cargar, etc.)

    Referencias de jQuery: cuando se quiere hacer una ref. sobre jq la sintaxis es:
        $("#identificadorDelElemento")

*/


(function (window, $) {

    const ns = window.Tasks.index = window.Tasks.index || {};
    ns.core = ns.core || {};

    // const container = document.getElementById("alertContainer");

    ns.core.$alert = $("#alertContainer"); //ns.core.nombreObjetoReferencia (variable llamada como alert)

    // ns.core.$alert.hide(); // referencia al mismo elmento con todas las funcione pero jQ 


})(window, jQuery);






/* ASIGNACIÓN 

- migrar todoo lo que se requiera de task-modal.js a lo que corresponda al core.js
    + elementos, referencias, variables y rutas
    + 
    + como no se modifica task-model, solo se copian las referencias, (mo se modifica), 
    solo se copian referencias una vez, 
    + migrar variables que sea necesario migrar 
    + migrar las rutas

*/