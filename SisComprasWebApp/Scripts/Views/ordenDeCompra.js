
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
            "ajax": {
                url: getUrlConsultaArticulos(),
                type: 'GET'
            },
            "columns": [
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
            var data = tablaDeProductosActuales.row($(this).parents('tr')).data();
            location.href = "AddArticulo?articuloId=2&ordenDeCompraId=12"
        });
    }

    var inicializarEventos = function () {
        $('#enviar').click(function(){
            $("#dialogEnviar").dialog("open");
        })
        $('#eliminar').click(function () {
            $("#dialogEliminar").dialog("open");

        })
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
                            $(this).dialog("close");
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
                inicializarEventos()
            })
        }
    }

})()

ordenDeCompra.init()