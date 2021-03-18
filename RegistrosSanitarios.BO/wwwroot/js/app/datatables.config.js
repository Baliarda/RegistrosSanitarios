var dTablesOptions = {    
    searchin: true,    
    stateSave: true,
    stateDuration: 60 * 5,
    paging: true,   
    bSort: false,
    bSortClasses: false,
    ordering: true,
    processing: true,
    deferRender: true,
    search: {
        "caseInsensitive": true
    },    
    lengthMenu: [[10, 15, 25, 50, -1], [10, 15, 25, 50, "Todos"]],
    language: {
        processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Cargando...</span> ',
        search: "Filtrar:",
        lengthMenu: "Mostrar _MENU_ registros",
        //info: "Mostrando _START_ a _END_ de _TOTAL_ registros",        
        //infoEmpty: "Mostrando 0 a 0 de 0 registros",
        infoEmpty: "",
        infoFiltered: "(filtrados de un total de _MAX_ registros)",
        info: "Página _PAGE_ de _PAGES_",               
        loadingRecords: "Cargando...",
        zeroRecords: "No hay registros",
        emptyTable: "No hay registros",
        pagingType: "numbers",
        paginate: {            
            first: "Primero",
            previous: "Anterior",
            next: "Siguiente",
            last: "Último"
        },
        thousands: ".",
        decimal: ","
    }
};


var dTablesOptionsServer = {
    searchin: true,
    stateSave: true,
    stateDuration: 60 * 5,
    paging: true,
    bSort: false,
    bSortClasses: false,
    pageLength: 50,
    ordering: true,
    autoWidth: true,
    search: {
        "caseInsensitive": true
    },
    lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
    deferRender: true,
    //scrollY: 600,
    //scrollCollapse: true,
    //scroller: true,
    processing: true,
    serverSide: true,
    language: {
        processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Cargando...</span> ',
        search: "Filtrar:",
        lengthMenu: "Mostrar _MENU_ registros",
        //info: "Mostrando _START_ a _END_ de _TOTAL_ registros",        
        //infoEmpty: "Mostrando 0 a 0 de 0 registros",
        infoEmpty: "",
        infoFiltered: "(filtrados de un total de _MAX_ registros)",
        info: "Página _PAGE_ de _PAGES_",
        loadingRecords: "Cargando...",
        zeroRecords: "No hay registros",
        emptyTable: "No hay registros",
        pagingType: "numbers",
        paginate: {
            first: "Primero",
            previous: "Anterior",
            next: "Siguiente",
            last: "Último"
        },
        thousands: ".",
        decimal: ","
    }
};

function initBasicDataTables() {
    $(".loading-spinner").remove();
    $(".grd-container").show();
    if ($.fn.dataTable.isDataTable($('.table-grd'))) {
        table = $('.table-grd').DataTable();
        table.destroy();
    }

    $('.table-grd').DataTable(dTablesOptions);
}

$('.table-grd').on('init.dt', function () {
    $('.table-grd').removeAttr("style");
});

function formatNumber(n) {
    var numberCurrency = 0;
    var strNumberCurrency = "";

    if (n === null || n === undefined)
        numberCurrency = numeral(0);
    else
        numberCurrency = numeral(n);

    // 2 decimales
    strNumberCurrency = numberCurrency.format('$ 0,0.00')

    return strNumberCurrency;
}

/* Create an array with the values of all the checkboxes in a column */

$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    //console.log("hola");
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).prop('checked') ? '1' : '0';
    });
}
