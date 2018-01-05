var ordenCompra = (
    function () {
        var GRID_ARTICULOS_CONTAINER = '#articuloCargadoGrid';
        var PAGER_ARTICULOS_CONTAINER = '#articuloCargadoPager';
        var GRID_CONTAINER = '#ordenCompraGrid';
        var PAGER_CONTAINER = '#ordenCompraPager';
        var SELECT_PROVEEDOR = "#proveedores"
        var images = []
        var main = (
            function () {
                var articulos = []
                var articulosEliminados = []
                var DIALOG_CONTAINER = '#ordenCompraDialog';
                var DIALOG_CONTAINER_WIDTH = 500;
                var DIALOG_CONTAINER_MIN_HEIGTH = 300;
                var URL_DELETE = '/OrdenCompra/Delete';
                var URL_CHECK_DELETE = '/OrdenCompra/CheckDelete';
                var NOMBRE_ORDENCOMPRA = '#nombre';
               
                var permisos = false;
                

                initOrdenCompraGrid = function () {
                    images = []
                    var colNames = ['ID', 'Foto', 'Codigo', 'Nombre', /*'Descripcion',*/ /*'Cantidad',*/'Precio'];
                    var colModel = [
                        { name: 'ID', hidden: true, key: true },
                        {
                            name: 'Foto', width: 30, formatter: function (cellvalue, options, rowObject) {
                                images.push({ ID: rowObject.ID, Foto: cellvalue })
                                return "<img src='data:image/jpg;base64," + cellvalue + "' style='height:90px; width:90px'/>"
                            }
                        },
                        { name: 'Codigo', width: 30, sorttype: 'text' },
                        { name: 'Nombre', width: 40, sorttype: 'text' },
                        //{ name: 'Descripcion', width: 80, sorttype: 'text' },
                        //{ name: 'Cantidad', width: 30, sorttype: 'text' },
                        { name: 'Precio', width: 40, sorttype: 'text' },
                    ]
                    var gridMode = core.gridModes.Add
                    var grid = core.getBasicGrid(GRID_CONTAINER, getURLOrdenCompraGRID(), colNames, colModel, null, 400, 10, PAGER_CONTAINER, gridMode, false);
                    core.setButtonAddFunction(GRID_CONTAINER, function () {
                        var selr = core.isSelectedRow(GRID_CONTAINER);
                        var id = core.getCellValue(GRID_CONTAINER, selr, "ID")
                        ordenCompra.main.showAdd(id);
                    });
                    core.setButtonEditFunction(GRID_CONTAINER, function (selr) { ordenCompra.main.showEdit(selr); });
                    //if (gridMode == core.gridModes.AddEdit)
                    //    core.addCustomButton(GRID_CONTAINER, PAGER_CONTAINER, 'navButtonEliminar', 'ui-icon-eliminarcustom', 'Eliminar fila seleccionada', function (selr) { ordenCompra.main.showDelete(selr); });
                   
                }


                initArticulosSeleccionadosGrid = function () {
                    let GRID_CONTAINER = ""
                    var colNames = ['ID', 'Foto', 'FotoUrl', 'Codigo', 'Nombre', /*'Descripcion',*/ 'Cantidad', 'Precio Unitario', 'Subtotal'];
                    var colModel = [
                        { name: 'ID', hidden: true, key: true },
                        {
                            name: 'Foto', width: 50, formatter: function (cellvalue, options, rowObject) {
                                return "<img src='data:image/jpg;base64," + cellvalue + "' style='height:90px; width:90px'/>"
                            }
                        },
                        { name: 'FotoUrl', hidden: true},
                        { name: 'Codigo', width: 40, sorttype: 'text' },
                        { name: 'Nombre', width: 40, sorttype: 'text' },
                        //{ name: 'Descripcion', width: 80, sorttype: 'text' },
                        { name: 'Cantidad', width: 30, sorttype: 'text' },
                        { name: 'Precio', width: 40, sorttype: 'text' },
                        { name: 'Subtotal', width: 40, sorttype: 'text' },
                    ]
                    var gridMode = core.gridModes.Add
                    var grid = core.getBasicGrid(GRID_ARTICULOS_CONTAINER, getURLOrdenCompraGRID(), colNames, colModel, null, 400, 10, PAGER_ARTICULOS_CONTAINER, gridMode, false);
                    core.setButtonAddFunction(GRID_ARTICULOS_CONTAINER, function () {
                        var selr = core.isSelectedRow(GRID_ARTICULOS_CONTAINER);
                        var id = core.getCellValue(GRID_ARTICULOS_CONTAINER, selr, "ID")
                        ordenCompra.main.showAdd(id);
                    });
                    core.setButtonEditFunction(GRID_ARTICULOS_CONTAINER, function (selr) { ordenCompra.main.showEdit(selr); });
                    core.addCustomButton(GRID_ARTICULOS_CONTAINER, PAGER_ARTICULOS_CONTAINER, 'navButtonEliminar', 'ui-icon-eliminarcustom', 'Eliminar fila seleccionada', function (selr) { ordenCompra.main.showDelete(selr); });
                }
                
                getURLOrdenCompraGRID = function () {
                    return '/OrdenCompra/ConsultarArticulosCargados' + getFilter();
                }

                getFilter = function () {
                    return '?sProveedorId=' + $(SELECT_PROVEEDOR).val();
                }
                
                initValidadores = function () {
                    $(NOMBRE_ORDENCOMPRA).attr('maxlength', '50');
                };
                
                initButtonsEvents = function () {

                    var BUSCAR_BUTTON = '#btnCargarArticulos';
                    //var BUTTON_CLEAN = '#limpiar';
                    //var INPUT_NOMBRE = '#nombre';
                    var FECHA_DESDE = "#FechaCargaDesde"
                    var FECHA_HASTA = "#FechaCargaHasta"

                    var GUARDAR_BUTTON = "#guardar"

                    $(BUSCAR_BUTTON).click(function (e) {
                        e.preventDefault();
                        images = []
                        core.reloadGrid(GRID_CONTAINER, getURLOrdenCompraGRID());
                    });

                    $(GUARDAR_BUTTON).click(function () {
                        let CABECERA = "#CabeceraId" 
                        let orden = {
                            CabeceraId: $(CABECERA).val(),
                            ProveedorId: $(SELECT_PROVEEDOR).val(),
                            Referencia: $("#NroReferencia").val(),
                            Cantidad: $("#CantidadTotal").val(),
                            MonedaId: $("#Moneda").val(),
                            ImporteTotal: $("#ImporteTotal").val(),
                            FechaEmision: $("#FechaEmision").val(),
                            Observaciones: $("#Observaciones").val(),
                            Cotizacion: $("#Cotizacion").val(),
                            Lineas: JSON.stringify(articulos),
                            Eliminados: JSON.stringify(articulosEliminados)
                        }
                        
                        $.ajax({
                            url: "/OrdenCompra/Guardar",
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({ ordenDto: orden }),
                            success: function () {
                                location.href = "Index"
                            },
                            error: function (jqXHR, exception, e) {
                                alert('Error message.');
                            }
                        });
                    })

                    //Evento keyDown del textbox filtro. Recarga la grilla al presionar enter                              
                    //$(INPUT_NOMBRE).keydown(function (e) {
                    //    if (e.keyCode == 13) {
                    //        e.preventDefault();
                    //        core.reloadGrid(GRID_CONTAINER, getURLOrdenCompraGRID());
                    //    }
                    //})

                    //$(BUTTON_CLEAN).click(function (e) {
                    //    e.preventDefault();
                    //    $(INPUT_NOMBRE).val('');
                    //    core.reloadGrid(GRID_CONTAINER, getURLOrdenCompraGRID());
                    //});
                }


                return {
                    initPage: function () {
                        //permisos = core.getPermisos(2);
                        initOrdenCompraGrid();
                        initArticulosSeleccionadosGrid()
                        initButtonsEvents();
                        //initValidadores();
                    },
                    showAdd: function (selr) {
                        core.showDialog(DIALOG_CONTAINER, 'Agregar Articulo', DIALOG_CONTAINER_WIDTH, DIALOG_CONTAINER_MIN_HEIGTH);
                        dialog.loadDialog(DIALOG_CONTAINER, selr);
                    },
                    showEdit: function (selr) {
                        core.showDialog(DIALOG_CONTAINER, 'Editar Articulo', DIALOG_CONTAINER_WIDTH, DIALOG_CONTAINER_MIN_HEIGTH);
                        dialog.loadDialog(DIALOG_CONTAINER, selr);
                    },
                    showDelete: function (selr) {
                        var selr = core.isSelectedRow(GRID_CONTAINER);
                        if (selr) {
                            core.showPopUpMessageConfirmation('Confirme', '¿Desea eliminar el registro seleccionado?', function () {
                                ordenCompra.main.articulosEliminados = selr
                                ordenCompra.main.eliminar(selr);
                            });
                        }
                    },
                    eliminar: function (selr) {
                        let pos = -1
                        ordenCompra.main.articulos.forEach(function (a, i) {
                            if (a.ID == selr) {
                                pos = i
                            }
                        })
                        ordenCompra.main.articulos.splice(pos, 1)
                        //core.reloadGrid(GRID_CONTAINER, getURLOrdenCompraGRID());
                        core.showPopUpMessageInformation('Artículo eliminado', 'Artículo eliminado con éxito');   
                    },
                    isValidForm: function (selr) {
                        core.reloadGrid(GRID_CONTAINER, getURLOrdenCompraGRID());
                        $(DIALOG_CONTAINER).dialog('close');
                    },
                    isInvalidForm: function (selr) {
                        dialog.initPage();
                    },
                    articulos: articulos
                }

            })();

        var dialog = (
            function () {
                var DIALOG_URL = '/OrdenCompra/AddArticle';
                var DIALOG_CONTAINER_TARGET = '#ordenCompraDialogTarget';
                let rowId = -1

                var actualizarTotales = function () {

                    var cotizacion = parseFloat($("#Cotizacion").val())
                    
                    var totales = ordenCompra.main.articulos
                        .map(function (l) {
                            return {
                                Cantidad: parseFloat(l.Cantidad),
                                Precio: cotizacion > 0 ? l.Precio * cotizacion : l.Precio,
                            }
                        })
                        .map(function (l) {
                        return {
                            Cantidad: l.Cantidad,
                            Subtotal: l.Cantidad * l.Precio
                        }
                        })
                        .reduce(function (total, linea) {
                        return {
                            Cantidad: total.Cantidad + linea.Cantidad,
                            Subtotal: total.Subtotal + linea.Subtotal
                        }
                    })

                    $("#CantidadTotal").val(totales.Cantidad)
                    $("#ImporteTotal").val(totales.Subtotal)
                }

                var agregarLinea = function (rowId, linea) {
                    let data = {
                        ID: core.getCellValue(GRID_CONTAINER, rowId, "ID"),
                        Foto: images.filter(function (o) { return o.ID == rowId; })[0].Foto,
                        Codigo: core.getCellValue(GRID_CONTAINER, rowId, "Codigo"),
                        Nombre: core.getCellValue(GRID_CONTAINER, rowId, "Nombre"),
                        Cantidad: linea.Cantidad,
                        Precio: core.getCellValue(GRID_CONTAINER, rowId, "Precio")
                    }
                    let cotizacion = $("#Cotizacion").val()
                    let precio = cotizacion > 0 ? data.Precio * cotizacion : data.Precio
                    data.Subtotal = precio * data.Cantidad
                    $(GRID_ARTICULOS_CONTAINER).jqGrid("addRowData", ordenCompra.main.articulos.length, data)
                }

                //Asigna los eventos a los controles
                initControlEventsDialog = function () {
                    var initEvents = (
                           function () {
                               return {
                                   //Evento click del boton cancelar
                                   initCancelClick: function () {
                                       var AGREGAR_BUTTON = "#btnAgregarArticulo"
                                       var CANCEL_BUTTON = '#cancelar';
                                       var DIALOG_CONTAINER = '#ordenCompraDialog';
                                       $(AGREGAR_BUTTON).click(function (e) {
                                           let linea = {
                                               ID: $("#LineaId").val(),
                                               Codigo: $("#Codigo").val(),
                                               ArticuloId: $("#ArticuloId").val(),
                                               Cantidad: parseFloat($("#Cantidad").val()),
                                               Precio: parseFloat(core.getCellValue(GRID_CONTAINER, rowId, "Precio"))
                                           }

                                           let lineaExistente = ordenCompra.main.articulos.filter(function (l) {
                                               return l.Codigo == linea.Codigo
                                           })
                                           lineaExistente = lineaExistente.length > 0 ? lineaExistente[0] : null
                                           if (lineaExistente) {
                                               lineaExistente.Cantidad += linea.Cantidad
                                               linea = lineaExistente
                                               
                                               $(GRID_ARTICULOS_CONTAINER).jqGrid('clearGridData')
                                               ordenCompra.main.articulos.forEach(function (l) {
                                                   agregarLinea(rowId, linea)
                                               })
                                           } else {
                                               ordenCompra.main.articulos.push(linea)

                                               agregarLinea(rowId, linea)
                                           }

                                           actualizarTotales()
                                           $(SELECT_PROVEEDOR).attr("disabled", "disabled")
                                           $(DIALOG_CONTAINER).dialog("close")
                                       })
                                       $(CANCEL_BUTTON).click(function (e) {
                                           e.preventDefault();
                                           core.showPopUpMessageConfirmation('Confirme', 'Se van a perder los cambios realizados ¿Desea continuar?', function () { $(DIALOG_CONTAINER).dialog('close'); });
                                       });
                                   }
                               }
                           })();

                    initEvents.initCancelClick();

                    initValidadores = function () {
                        var CODIGO = '#Codigo';
                        var NOMBRE = '#Nombre';

                        $(CODIGO).attr('maxlength', '2');
                        $(NOMBRE).attr('maxlength', '50');
                        $(CODIGO).keydown(function (event) {
                            event = event || window.event;
                            var codigo = event.keyCode ? event.keyCode : event.charCode;
                            if (codigo == 32) {
                                event.preventDefault()
                            }
                        });
                    };
                }
                return {
                    loadDialog: function (dialogContainer, selr) {
                        rowId = selr
                        core.loadDialogPage(DIALOG_URL, dialogContainer, DIALOG_CONTAINER_TARGET, { id: selr, cabeceraId: 0 }, false, dialog.initPage);
                        core.setFocus('#Codigo')
                    },
                    initPage: function () {
                        initControlEventsDialog();
                        initValidadores();
                    }
                }
            })();
        return {
            main: main
        }
    })();

ordenCompra.main.initPage();