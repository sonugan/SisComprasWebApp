/// <reference path="jquery-1.7.1.intellisense.js" />
/// <reference path="jquery-1.5.1-vsdoc.js" />

var loadingCount = 0;

$(document).ajaxStart(function () {
    cargarLoad();    
});

$(document).ajaxStop(function () {
    descargarLoad();
});

function cargarLoad() {
    loadingCount++;
    $.blockUI({ message: '<h1><br/>Cargando...<br/></h1>', css: { backgroundColor: '#DCDCDC' } });
}

function descargarLoad() {
    loadingCount--;
    if (loadingCount == 0) {
        $.unblockUI();
    }
}

function forzarDescarga() {
    $.unblockUI();
}