/*
CORE.JS = 
    - Moódulo para definición de variables (namespace, url de módulos...), archivos compartidos, variables de contexto...
    - Selectores comunes son identificados como "ns.selectores" ( o "ns.$name" 

    !! NO almacena handles, de evento, ajax, lógica de negocio, funciones ni procesos completos (como guardar, cargar, etc.)

    Referencias de jQuery: cuando se quiere hacer una ref. sobre jq la sintaxis es:
        $("#identificadorDelElemento")

*/



/* ASIGNACIÓN 

- migrar todoo lo que se requiera de task-modal.js a lo que corresponda al core.js
    + elementos, referencias, variables y rutas
    + 
    + como no se modifica task-model, solo se copian las referencias, (mo se modifica), 
    solo se copian referencias una vez, 
    + migrar variables que sea necesario migrar 
    + migrar las rutas

    - terminar core.js
    - migración api.js
*/


/*
(function (window, $) {

    // namespace
    const ns = window.Tasks.index = window.Tasks.index || {};
    ns.core = ns.core || {};

    // rutas (URLs del módulo): todas las llamadas a los Controllers 'TasksController' y 'Categories'
    ns.core.urls = {
        createPartial: '/Tasks/CreatePartial',
        editPartial: '/Tasks/EditPartial/',
        create: '/Tasks/Create',
        edit: '/Tasks/Edit/',
        delete: '/Tasks/DeleteAjax/',
        loadTable: '/Tasks/LoadTablePartial',
        categoryOptions: '/Categories/Options'
    };

    // selectores y elementos (ns.selectores & ns.$name) -> se usa $ para indicar cuando es objeto jQuery 
    ns.core.selectors = {
        // "ID´s"" del Modal
        taskModal: '#taskModal',
        modalContent: '#taskModalContent',

        // botones y Formularios
        btnCreate: '#btnCrearTask',
        btnSave: '#btnSaveTask',
        taskForm: '#taskForm',

        // contenedores y otros
        taskTableContainer: '#taskTableContainer',
        alertContainer: '#alertContainer',
        $categorySelect: $('#categorySelect') // modificación para mantener el array, colocando la sintaxis $ para señalizar (variale) y su referencia al objeto tipo jQuery

    };
    ns.core.selectors.$btnCreate = $("#btnCrearTask"); // ejemplo como referencia en lugar de un Id

    // Referencias a elementos comunes (Variables de contexto)
    ns.core.$alert = $(ns.core.selectors.alertContainer);
    ns.core.$taskTable = $(ns.core.selectors.taskTableContainer);

})(window, jQuery);
*/







/* OBSERVACIONES - SESIÓN 31 MARZO:

    con el Id Js asume que hay un sólo elemento con dicho identificador, sin embargo, en la página solo debería haber un 
    elemento con dicho Id, puesto que Js es Interpretado, a leer de arriba abajo la última mención o referencia declarada 
    será la que se tome.


    Con la Clase, al ser más general, no importa dónde sea declarada, ya que en cada mención se tratará para una cosa en 
    particular que no afecta la mención del resto de las menciones con sus respectivas interacciones. 

    Por ende, la declaración no es estrictamente errónea, pero están pensadas en términos de usabilidad diferentes. 
    Por ejemplo, puede darse el caso en el que dentro del mismo flujo se requiera llamar al mismo botón, 
        en una se usa para capturar el clic y en otra puede ques se requiera otra interacción para colocar una propiedad
        como 'disabled' para inhanilitarlo momentáneamente 



    Dicho lo anterior, en el array 'ns.core.selectors' se debe pensar en el uso posible que pueden tener los elementos que
    lo conforman para declararlos adecuadamente (como Id o como Clase).

    En el ejemplo agregado de la línea: ns.core.selectors.$btnCreate = $("#btnCrearTask");
    -> Se tiene un objeto nombrado como $btnCreate que contiene una REFERENCIA al objeto del DOM: $("#btnCrearTask");
    -> al tener dicha referencia, se tiene en realidad el elemento junto con el ID, clases, atributos y todo lo que corresponda 

    En el segundo ejemplo:
        ns.core.selectors.$btnCreate.on('click', async function () {
        ns.core.selectors.$btnCreate.prop("disabled", true);
        });
    Funge como código demostrativo que iría en el archivo events.js, sin embargo, se ejemplifica que lo que el código indica
    qué se requiere capturar cuando el usuario interactúe con el botón dándole clic, y después se desactiva el botón para no 
    volver a capturar el evento.

    De esta manera se demuestra cómo se usa el mismo elemento/objeto/variable que se tiene en el namespace ($btnCreate) 
    -> sin tener que crearla 2 veces, ni obtener la referencia 2 veces, sino únicamente reusarla
    ✔️ Más aprovechable 

    En el tercer ejemplo:
        $(ns.core.selectors.btnCreate).on('click', async function () {
        $(ns.core.selectors.btnCreate).prop("disabled", true);
        });
    Se tiene que buscar la referencia en los 2 escenarios, de manera que se indique a jQuery que "con dicho Id busque en todo
    el DOM un elemento que coincida con dicho Id y capture el evento, y después, en la siguiente línea, se indica que vuelva
    a buscar el elmento por medio de su identificador y modifique su propiedad (.prop) al estado 'disabled' "
    -> puede generar errores tipo typo al tener que ser reescrito de manera más precisa, pues cada mención se convierte en 
        un elemento diferente si no se escribe exactamente igual, inherente a lo que es un Id
    ✔️ Ligera tendencia a errores tipo typo conforme al comportamiento
*/

(function (window, $) {
    const ns = window.Tasks.index = window.Tasks.index || {};
    ns.core = ns.core || {};

    ns.core.urls = {
        createPartial: '/Tasks/CreatePartial',
        editPartial: '/Tasks/EditPartial/',
        create: '/Tasks/Create',
        edit: '/Tasks/Edit/',
        delete: '/Tasks/DeleteAjax/',
        loadTable: '/Tasks/LoadTablePartial',
        categoryOptions: '/Categories/Options'
    };

    // Referencias/Id (#) para lo fijo, Selectores para lo dinámico ($)
    ns.core.selectors = {

        $modalContent: $('#taskModalContent'),
        $taskForm: $('#taskForm'),
        // Referencias (objetos jQuery $) -> se mandan a llamar como ns.core.selectors.$...;
        $taskModal: $('#taskModal'),
        $btnCreate: $('#btnCrearTask'),
        $btnSave: $('#btnSaveTask'),
        $taskTable: $('#taskTableContainer'),
        $alert: $('#alertContainer')

    };

})(window, jQuery);
