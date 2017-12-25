
var ordenDeCompra = (function () {

    var getUrlConsultaArticulos = function () {
        return 'ConsultarArticulosCargados?sProveedorId=' + $('#cabecera_ProveedorId').val() + "&sFechaCargaDesde=" + $('#FechaCargaDesde').val() + "&sFechaCargaHasta=" + $('#FechaCargaHasta').val()
    }

    var tablaDeProductosActuales;

    var inicializarListaDeProductosActuales = function () {
        tablaDeProductosActuales = $('#productosActuales').DataTable({
            "processing": true,
            "serverSide": true,
            "bFilter": false,
            "bInfo": false,
            "ajax": {
                url: getUrlConsultaArticulos(),
                type: 'GET'
            },
            "columns": [
               { "data": "ID", "visible": false },
               { "data": "Foto" },
               { "data": "Codigo" },
               { "data": "Nombre" },
               { "data": "Descripcion" },
               {
                   "data": null,
                   "defaultContent": "<a title='Agregar artículo'> <img src='/Content/Imagenes/check.png' style='width:80px;height:80px'></a>"
               }
            ],
            "language": {
                "decimal": "",
                "emptyTable": "No hay datos disponibles en la tabla",
                "info": "Mostrando de _START_ a _END_ de _TOTAL_ registros",
                "infoEmpty": "Mostrando 0 a 0 de 0 registros",
                "infoFiltered": "(filtered from _MAX_ total entries)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrando _MENU_ registros",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "No se han encontrado registros coincidentes",
                "paginate": {
                    "first": "Primero",
                    "last": "Último",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                }
            }
        });
        $('#productosActuales tbody').on('click', 'a', function (e) {
            e.preventDefault()
            var data = tablaDeProductosActuales.row($(this).parents('tr')).data()
            let cabeceraId = $("#CabeceraId").val()
            location.href = "AddArticulo?articuloId=" + data.ID + "&ordenDeCompraId=" + cabeceraId
        });
    }

    var inicializarEventos = function () {
        $('#enviar').click(function(){
            $("#dialogEnviar").dialog("open")

        })
        $('#eliminar').click(function () {
            $("#dialogEliminar").dialog("open");

        })
    }

    var mostrarError = function (mensaje) {
        $("#dialogError #mensaje").text(mensaje)
        $("#dialogError").dialog("open")
    }

    return {
        init: function () {
            $(document).ready(function () {
                if (recargarGrilla) {
                    inicializarListaDeProductosActuales()
                }
                $("#btnCargarArticulos").click(function () {
                    if (tablaDeProductosActuales) {
                        tablaDeProductosActuales.destroy()
                    }
                    inicializarListaDeProductosActuales()
                })
                $("#dialogEnviar").dialog({
                    autoOpen: false, modal: true, buttons: {
                        "Enviar": function () {
                            let cabeceraId = $("#CabeceraId").val()
                            let me = this
                            $.get("Enviar?ordenDeCompraId=" + cabeceraId, (data) => {
                                if (data == "ok") {
                                    $(me).dialog("close");
                                } else {
                                    $(me).dialog("close");
                                    mostrarError("Se ha producido un error enviando la orden")
                                }
                            })
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogEliminar").dialog({
                    autoOpen: false, modal: true, buttons: {
                        "Eliminar": function () {
                            $(this).dialog("close");
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogError").dialog({
                    autoOpen: false, modal: true, buttons: {
                        "OK": function () {
                            $(this).dialog("close");
                        }
                    }
                })
                inicializarEventos()
            })
        }
    }

})()

ordenDeCompra.init()