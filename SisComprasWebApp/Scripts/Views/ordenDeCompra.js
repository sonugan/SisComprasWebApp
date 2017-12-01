
var ordenDeCompra = (function () {

    var getUrlConsultaArticulos = function () {
        return 'ConsultarArticulosCargados?sProveedorId=' + $('#cabecera_ProveedorId').val() + "&sFechaCargaDesde=" + $('#FechaCargaDesde').val() + "&sFechaCargaHasta=" + $('#FechaCargaHasta').val()
    }

    var tablaDeProductosActuales;

    var inicializarListaDeProductosActuales = function () {
        tablaDeProductosActuales = $('#productosActuales').DataTable({
            "processing": true,
            "serverSide": true,
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
                    "defaultContent": "<button>Click!</button>"
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
        $('#productosActuales tbody').on('click', 'button', function (e) {
            e.preventDefault()
            var data = tablaDeProductosActuales.row($(this).parents('tr')).data();
            location.href = "AddArticulo?articuloId=2&ordenDeCompraId=12"
        });
    }

    return {
        init: function () {
            $(document).ready(function () {
                $("#btnCargarArticulos").click(function () {
                    if (tablaDeProductosActuales) {
                        tablaDeProductosActuales.destroy()
                    }
                    inicializarListaDeProductosActuales()
                })
            })
        }
    }

})()

ordenDeCompra.init()