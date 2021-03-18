$('#body-content').on('change keyup keydown', 'input, textarea, select, checkbox', function (e) {
    //console.log("cambio");
    $(this).addClass('changed-input');
    $('[data-toggle=confirmation]').confirmation({
        rootSelector: '[data-toggle=confirmation]',

    });
});
$('#Volver').click(function () {
    if ($('.changed-input').length) {
        $('#element').confirmation('show');

    }
    else {
        $('#element').confirmation('hide');
    }

});

$(function () {

    $.validator.methods.date = function (value, element) {
        //CGRAPPA: Si esta vacia la fecha considerarla valida, si es requerida hay otro validador
        var validFormat = (value === "" || moment(value, ['MM/DD/YYYY', 'DD/MM/YYYY', 'YYYY-MM-DD'], true).isValid());
        return validFormat;
    },
        "Ingrese una fecha con formato dd/mm/yyyy."
})

jQuery.extend(jQuery.validator.messages, {
    date: "Ingrese una fecha con formato dd/mm/yyyy.",
    max: jQuery.validator.format("Ingrese una fecha válida con formato dd/mm/yyyy"),
    min: jQuery.validator.format("Ingrese una fecha válida con formato dd/mm/yyyy")
});

function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}

document.onkeypress = stopRKey;



