﻿
var articulosCargados = (function () {

    var getUrlConsultaArticulos = function () {
        return 'ConsultarArticulosCargadosEnOrden?cabeceraId=' + $('#cabeceraId').val();
    }

    var tablaDeProductosActuales;
    var articuloEliminar = null
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
                { "data": "ID" },
                { "data": "Foto" },
                { "data": "Codigo" },
                { "data": "Nombre" },
                { "data": "Descripcion" },
                { "data": "Cantidad" },
                { "data": "Precio" },
                {
                    "data": null,
                    "defaultContent": "<a title='Remover artículo'> <img src='/Content/Imagenes/delete.png' style='width:80px;height:80px'></a>"
                },
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
            let data = tablaDeProductosActuales.row($(this).parents('tr')).data()
            let cabeceraId = $("#CabeceraId").val()
            articuloEliminar = data
            $("#dialogEliminar").dialog("open");
        });
    }

    return {
        init: function () {
            $(document).ready(function () {
                if (tablaDeProductosActuales) {
                    tablaDeProductosActuales.destroy()
                }
                inicializarListaDeProductosActuales()
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
                            var data = articuloEliminar
                            $.get("EliminarArticulo?articuloId=" + data.ID, function (data) {
                                if (data == "ok") {
                                    tablaDeProductosActuales.destroy()
                                    inicializarListaDeProductosActuales()
                                    $(me).dialog("close");
                                } else {
                                    $(me).dialog("close");
                                    mostrarError("Se ha producido un error eliminando el articulo")
                                }
                            })
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            })
        }
    }

})()

articulosCargados.init()