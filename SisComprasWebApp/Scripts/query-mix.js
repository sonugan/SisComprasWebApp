$(document).ready(function () {

    //Carga los estilos y los eventos
    queryLoader.loadQuery();
});

var queryLoader = (function () {

    /* Función a ejecutar cuando la página cambia de tamaño */
    function paginaFluida() {
        if ($(window).width() > 1280) {
            var anchoJQGrid = 1178;
            var anchoJQGridEnPopup = 1100;
        } else {
            var anchoJQGrid = 917;
            var anchoJQGridEnPopup = 800;
        }
        $("#list00, #list01, #list02, #list04").setGridWidth(anchoJQGrid - 45);
        $("#list03").setGridWidth(anchoJQGrid);
        $("#list05").setGridWidth(anchoJQGridEnPopup - 75);
    }

    /* Ejecuta funciones al cambiar de tamaño la página */
    $(window).resize(function () {
        setTimeout(function () {
            paginaFluida();
        }, 200);
    });

    function loadQuery() {
        loadStyles();
        /* Menu */
        $('nav ul>li>a').wrapInner('<div class="texto"></div>');
        $('nav .submenu>li:first-child div').addClass('first');
        $('nav>ul>li>a').prepend('<div class="inicioBoton"></div>');
        $('nav>ul>li>a').append('<div class="cierreBoton"></div>');
        $('.submenu .texto').mouseenter(function () {
            $(this).children('span').fadeIn(300);
        });
        $('.submenu .texto').mouseleave(function () {
            $(this).children('span').stop(true, true).fadeOut(150);
        });
        $('nav li').mouseenter(function () {
            $(this).children('ul').fadeIn(400);
        });
        $('nav li').mouseleave(function () {
            $(this).children('ul').stop(true, true).fadeOut(200);
        });
        /* Breadcrum */
        $('.breadcrum li').before('<li><span class="fa fa-angle-right"></span></li>');
        $('.breadcrum').prepend('<li><a href="/" title=""><span class="fa fa-home"></span></a></li>');
        /* jqGrid reemplazo de iconos */
        changeImageButtons();

        /* Tooltips simple */
        $('.tooltip').each(function () {
            $(this).qtip({
                content: {
                    text: $(this).next('.tooltiptext')
                },
                position: {
                    my: 'bottom centert',
                    at: 'top center',
                    target: 'mouse',
                    adjust: { x: 0, y: -10 }
                },
                style: {
                    classes: 'qtip-light qtip-shadow qtip-rounded'
                }
            });
        });
        /* Tooltips con titulo */
        $('.tooltipTitulo').each(function () {
            $(this).qtip({
                content: {
                    text: $(this).next('.tooltiptext'),
                    title: function (event, api) {
                        return $(this).attr('alt');
                    }
                },
                position: {
                    my: 'bottom centert',
                    at: 'top center',
                    target: 'mouse',
                    adjust: { x: 0, y: -10 }
                },
                style: {
                    classes: 'qtip-green qtip-shadow qtip-rounded'
                }
            });
        });
        /* Verifica cantidad de botones en el portal de acceso y asigna una clase específica */
        var cantidadLi = $('.contenedorBotones li').length;
        if (cantidadLi > 3) {
            $('.contenedorBotones').removeClass('pocosBotones');
            $('.contenedorBotones').addClass('muchosBotones');
        } else {
            $('.contenedorBotones').removeClass('muchosBotones');
            $('.contenedorBotones').addClass('pocosBotones');
        }
        ;
        if (cantidadLi === 1) {
            $('.contenedorBotones ul').css({ "margin-top": "120px", "margin-bottom": "150px" });
        }
        ;
        if (cantidadLi === 2) {
            $('.contenedorBotones ul').css({ "margin-top": "80px", "margin-bottom": "100px" });
        }
        ;
        if (cantidadLi === 4) {
            $('.contenedorBotones ul').css({ "margin-top": "20px", "margin-bottom": "20px" });
        }
        ;

        /* Ejecución de función por primera vez */
        paginaFluida();
    }

    /* jqGrid reemplazo de iconos */
    function changeImageButtons() {

        $(".ui-pg-div .ui-icon-plus")
             .removeClass("ui-icon ui-icon-plus")
             .addClass("fontawesome fa fa-plus-circle");
        $(".ui-icon-pencil")
             .removeClass("ui-icon ui-icon-pencil")
             .addClass("fontawesome fa fa-pencil");
        $(".ui-icon-trash")
             .removeClass("ui-icon ui-icon-trash")
             .addClass("fontawesome fa fa-trash-o");

        $(".ui-icon-search")
             .removeClass("ui-icon ui-icon-search")
             .addClass("fontawesome fa fa-search");

        $(".ui-icon-arrowstop-1-s")
             .append("<div class='caption'>Excel</div>")
             .removeClass("ui-icon ui-icon-arrowstop-1-s")
             .addClass("fontawesome fa fa-download");

        $(".ui-icon-detalles")
            .append("<div class='caption'>Detalles</div>")
            .removeClass("ui-icon ui-icon-detalles")
            .addClass("fontawesome fa fa-file-text-o");

        $(".ui-icon-ajustes")
            .append("<div class='caption'>Ajustes</div>")
             .removeClass("ui-icon ui-icon-ajustes")
             .addClass("fontawesome fa fa-pencil");

        $(".ui-icon-bajarValidacion")
        .append("<div class='caption'>Bajar</div>")
        .removeClass(".ui-icon-bajarValidacion")
        .addClass("fontawesome fa fa-caret-square-o-down");

        $(".ui-icon-recomendaciones")
            .append("<div class='caption'>Recomendaciones</div>")
            .removeClass("ui-icon .ui-icon-recomendaciones")
            .addClass("fontawesome fa fa-book");
        $(".ui-icon-datoscomite")
            .append("<div class='caption'>Datos Comité</div>")
            .removeClass("ui-icon .ui-icon-datoscomite")
            .addClass("fontawesome fa fa-users");
        $(".ui-icon-folder-collapsed")
            .append("<div class='caption'>Finalizados</div>")
            .removeClass("ui-icon ui-icon-folder-collapsed")
            .addClass("fontawesome fa fa-folder-open");
        //botones cabeceras en importar a sap
        $(".ui-icon-gpoperacional")
            .append("<div class='caption'>GP Operacional</div>")
            .removeClass("ui-icon ui-icon-gpoperacional")
            .addClass("fontawesome fa fa-file-text-o");

        $(".ui-icon-historico")
            .append("<div class='caption'>Históricos</div>")
            .removeClass("ui-icon ui-icon-historico")
            .addClass("fontawesome fa fa-bars");

        $(".ui-icon-activo")
            .append("<div class='caption'>Activar</div>")
            .removeClass("ui-icon ui-icon-activo")
            .addClass("fontawesome fa fa-bolt");

        $(".ui-icon-gpotransito")
            .append("<div class='caption'>GP Transito</div>")
            .removeClass("ui-icon ui-icon-gpotransito")
            .addClass("fontawesome fa fa-file-text");

        $(".ui-icon-desvio")
            .append("<div class='caption'>Desvío</div>")
            .removeClass("ui-icon ui-icon-gpotransito")
            .addClass("fontawesome fa fa-file-text");

        $(".ui-icon-enviarajustificar")
        .append("<div class='caption'>Enviar</div>")
            .removeClass("ui-icon ui-icon-enviarajustificar")
        .addClass("fontawesome fa fa-envelope");

        $(".ui-icon-eliminarcustom")
        .append("<div class='caption'></div>")
        .removeClass("ui-icon ui-icon-eliminarcustom")
        .addClass("fontawesome fa fa-trash-o");

        $(".ui-icon-aprobar")
        .append("<div class='caption'>Aprobar</div>")
        .removeClass("ui-icon ui-icon-aprobar")
        .addClass("fontawesome fa fa-thumbs-o-up");

        $(".ui-icon-corridas")
        .append("<div class='caption'>Corridas</div>")
        .removeClass("ui-icon ui-icon-corridas")
        .addClass("fontawesome fa fa-chevron-circle-right");

        $(".ui-icon-revisar")
        .append("<div class='caption'>Revisar</div>")
            .removeClass("ui-icon ui-icon-revisar")
        .addClass("fontawesome fa fa-eye");

        $(".ui-icon-pdf-file")
       .append("<div class='caption'>PDF</div>")
           .removeClass("ui-icon ui-icon-pdf-file")
       .addClass("fontawesome fa fa-file-o");

        $(".ui-icon-importar")
             .addClass("fontawesome fa fa-cloud-upload fa-lg");
        $(".ui-icon-calc")
          .addClass("fontawesome fa fa-cogs fa-lg");
        //boton buscar y borrar//
        $(".ui-icon-search")
            .removeClass("ui-icon ui-icon-search")
            .addClass("fontawesome fa fa-search");
        $(".ui-icon-erase")
             .addClass("fontawesome fa fa-eraser fa-lg");
        $(".ui-icon-bulding")
     .addClass("fontawesome fa fa-map-marker");
        //boton check usuario
        $(".ui-icon-check-user")
             .addClass("fontawesome fa fa-user");

    }

    /* jqGrid reemplazo de iconos que se encuentran en un dialog*/
    function changeImageButtonsReload() {

        $(".ui-dialog .ui-pg-div .ui-icon-plus")
             .removeClass("ui-icon ui-icon-plus")
             .addClass("fontawesome fa fa-plus-circle");
        $(".ui-dialog .ui-icon-pencil")
             .removeClass("ui-icon ui-icon-pencil")
             .addClass("fontawesome fa fa-pencil");
        $(".ui-dialog .ui-icon-trash")
             .removeClass("ui-icon ui-icon-trash")
             .addClass("fontawesome fa fa-trash-o");

        $(".ui-dialog .ui-icon-arrowstop-1-s")
             .append("<div class='caption'>Excel</div>")
             .removeClass("ui-icon ui-icon-arrowstop-1-s")
             .addClass("fontawesome fa fa-download");

        $(".ui-dialog .ui-icon-bajarValidacion")
        .append("<div class='caption'>Bajar</div>")
        .removeClass("ui-icon .ui-icon-bajarValidacion")
        .addClass("fontawesome fa fa-caret-square-o-down");

        $(".ui-dialog .ui-icon-recomendaciones")
            .append("<div class='caption'>Recomendaciones</div>")
            .removeClass("ui-icon .ui-icon-recomendaciones")
            .addClass("fontawesome fa fa-book");
        $(".ui-dialog .ui-icon-datoscomite")
            .append("<div class='caption'>Datos Comité</div>")
            .removeClass("ui-icon .ui-icon-datoscomite")
            .addClass("fontawesome fa fa-users");
        $(".ui-dialog .ui-icon-folder-collapsed")
            .append("<div class='caption'>Finalizados</div>")
            .removeClass("ui-icon ui-icon-folder-collapsed")
            .addClass("fontawesome fa fa-folder-open");
        //botones cabeceras en importar a sap
        $(".ui-dialog .ui-icon-gpoperacional")
            .append("<div class='caption'>GyP Operacional</div>")
            .removeClass("ui-icon ui-icon-gpoperacional")
            .addClass("fontawesome fa fa-file-text-o");

        $(".ui-icon-detalles")
            .append("<div class='caption'>GP Operacional</div>")
            .removeClass("ui-icon ui-icon-detalles")
            .addClass("fontawesome fa fa-file-text-o");

        $(".ui-icon-ajustes")
           .append("<div class='caption'>Ajustes</div>")
            .removeClass("ui-icon ui-icon-ajustes")
            .addClass("fontawesome fa fa-pencil");

        $(".ui-dialog .ui-icon-gpotransito")
            .append("<div class='caption'>GyP Transito</div>")
            .removeClass("ui-icon ui-icon-gpotransito")
            .addClass("fontawesome fa fa-file-text");

        $(".ui-dialog .ui-icon-desvio")
            .append("<div class='caption'>Desvío</div>")
            .removeClass("ui-icon ui-icon-gpotransito")
            .addClass("fontawesome fa fa-file-text");

        $(".ui-icon-enviarajustificar")
        .append("<div class='caption'>Enviar</div>")
            .removeClass("ui-icon ui-icon-enviarajustificar")
        .addClass("fontawesome fa fa-envelope");

        $(".ui-icon-aprobar")
        .append("<div class='caption'>Aprobar</div>")
        .removeClass("ui-icon ui-icon-aprobar")
        .addClass("fontawesome fa fa-thumbs-o-up");

        $(".ui-icon-corridas")
.append("<div class='caption'>Corridas</div>")
.removeClass("ui-icon ui-icon-corridas")
.addClass("fontawesome fa fa-chevron-circle-right");

        $(".ui-dialog .ui-icon-eliminarcustom")
        .append("<div class='caption'>Eliminar</div>")
        .removeClass("ui-icon ui-icon-eliminarcustom")
        .addClass("fa fa-trash-o");

        $(".ui-dialog .ui-icon-revisar")
        .append("<div class='caption'>Revisar</div>")
            .removeClass("ui-icon ui-icon-revisar")
        .addClass("fontawesome fa fa-eye");

        $(".ui-icon-pdf-file")
            .append("<div class='caption'>PDF</div>")
            .removeClass("ui-icon ui-icon-pdf-file")
            .addClass("fontawesome fa fa-file-o");

        $(".ui-dialog .ui-icon-importar")
             .addClass("fontawesome fa fa-cloud-upload fa-lg");
        $(".ui-dialog .ui-icon-calc")
          .addClass("fontawesome fa fa-cogs fa-lg");
        //boton buscar y borrar//
        $(".ui-icon-search")
            .removeClass("ui-icon ui-icon-search")
            .addClass("fontawesome fa fa-search");
        $(".ui-dialog .ui-icon-erase")
             .addClass("fontawesome fa fa-eraser fa-lg");
        //boton check usuario
        $(".ui-dialog .ui-icon-check-user")
             .addClass("fontawesome fa fa-user");

    }

    function loadStyles() {
        /* Despliega y repliega las cajas de filtros */
        $('.iconoDespliega').click(function () {
            $(this).parent().parent().parent().find('.filtros').slideToggle("100");
            $(this).children("span").toggleClass("fa-chevron-down");
        });
        /* Comportamiento de campos del formulario */
        $("select.custom").each(function () {
            var sb = new SelectBox({
                selectbox: $(this),
                height: 120,
                width: 100
            });
        });
        $('input[type="checkbox"]').css({ 'opacity': 100 });
        $('input[type="radio"]').css({ 'opacity': 0 });
        /*$('input').customInput();*/
        $('.checkboxVerde').find('.custom-checkbox').addClass('custom-checkboxVerde-sLabel');
        $('.checkboxRojo').find('.custom-checkbox').addClass('custom-checkboxRojo-sLabel');
        $('.fileupload').filestyle({
            width: 100
        });
        $('input[type="file"].fileupload').focus(function () {
            $(this).prev().addClass("curFocus");
        });
        $('input[type="file"].fileupload').blur(function () {
            $(this).prev().removeClass("curFocus");
        });
        $('input[type="text"], select, textarea').focus(function () {
            $(this).addClass("focusjquery");
        });
        $('input[type="text"], select, textarea').blur(function () {
            $(this).removeClass("focusjquery");
        });

        loadStylesReload();

        /* Abro dialogos en tamaño "normal" o "chico" */
        $(".abreDialog, .abreDialogChico").click(function (event) {
            event.preventDefault();
            /* Averiguo datos del boton de apertura para definir que dialog usar, que tamaño debe tener y el color del encabezado */
            var atributoClass = $(this).attr('class').slice(0, 15);
            var atributoClassColor = $(this).attr('class').slice(-3);
            var idDialog = 'ID_' + $(this).attr('id');
            /* La medida de los dialogos depende de la medida de la ventana del browser */
            if ($(window).width() > 1200) {
                var anchoPopup = 1100;
                var altoPopup = 550;
            } else {
                var anchoPopup = 800;
                var altoPopup = 400;
            }
            if (atributoClass === "abreDialogChico") {
                anchoPopup = anchoPopup / 2;
                altoPopup = altoPopup / 2;
            }
            $("#" + idDialog).dialog({
                position: { my: "center", at: "center", of: window },
                width: anchoPopup,
                height: altoPopup,
                modal: true,
                resizable: false,
                show: {
                    effect: "fadeIn",
                    duration: 200
                },
                hide: {
                    effect: "fadeOut",
                    duration: 100
                },
                create: function (event, ui) {
                    var widget = $(this).dialog("widget");
                    $(".ui-button .ui-button-icon-primary", widget)
                            .removeClass("ui-button-icon-primary ui-icon ui-icon-closethick")
                            .addClass("fontawesome fa fa-times-circle");
                    /* Me permite tener un encabezado de color diferente */
                    if (atributoClassColor === "Fuc") {
                        $(widget).addClass("fucsia");
                    }
                }
            });
            $(window).resize(function () {
                $("#" + idDialog).dialog("option", "position", "center");
            });
        });
    }

    function loadStylesReload() {
        /* Remover mensajes del sistema */
        $(".removerMensaje").click(function (event) {
            event.preventDefault();
            $(this).parents(".mensaje").fadeOut("slow");
        });
        /* Datepicker */
        $(".settingdatepicker").datepicker({
            showOn: "button",
            buttonImage: "../Images/calendar.png",
            buttonImageOnly: true,
            buttonText: "Seleccione fecha",
            changeMonth: true,
            changeYear: true
        });
        /* Tabs*/
        $(".tabs").tabs();

        $(".soloLectura").keydown(function (e) { e.preventDefault() })
        $(".soloLectura").focus(function () { this.blur() })

    }

    return {
        loadQuery: loadQuery,
        changeImageButtons: changeImageButtonsReload,
        loadStylesReload: loadStylesReload
    }

})();