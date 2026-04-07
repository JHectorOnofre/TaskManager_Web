(function (window, $) {

    const ns = window.Tasks.index = window.Tasks.index || {};
    ns.events = ns.events || {};

    //referencia directa "btnSave"
    ns.core.selectors.btnSave.on("click", async function () {
        await ns.service.saveTask();
    }); 



    ns.init = function () {
    };
})(window, jQuery);