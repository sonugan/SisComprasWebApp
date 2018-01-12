// Core de js de la aplicación.
var core = (
    function () {
        // Constantes

        // Variables privadas
        var filesUpload;


        // Funciones privadas


        // Funciones públicas
        return {

            // Modos de visualización de grillas
            gridModes: {
                View: 0,
                AddEdit: 1,
                Add: 2,
                Edit: 3
            },

            // Configuración inicial de grilla
            // Parametros
            // gridContainer: Contenedor de la grilla
            // url: Url para la carga inicial de la grilla
            // colNames: Column Names en notación JSON
            // colModel: Column Model en notación JSON
            // sortName: Columna por la que se ordenará inicialmente
            // height: Alto que ocupará la grilla
            // rowNum: Cantidad de registros a mostrar por página
            // pager: Id del paginador de la grilla
            // gridMode: Valores posibles VIEW, EDIT
            // viewrecords
            getBasicGrid: function (gridContainer, url, colNames, colModel, sortName, height, rowNum, pager, gridMode, viewrecords, func, loadonce) {
                var grid = $(gridContainer);
                viewrecords = (viewrecords != null) ? viewrecords : false;
                loadonce = (loadonce != null) ? loadonce : true;
                grid.jqGrid({
                    url: url,
                    datatype: 'json',
                    colNames: colNames,
                    colModel: colModel,
                    height: height,
                    loadonce: loadonce,
                    autoencode: true,
                    gridview: true,
                    sortable: true,
                    autowidth: true,
                    sortname: sortName,
                    sortorder: 'asc',
                    multiselect: false,
                    viewrecords: viewrecords,
                    pager: pager,
                    rowNum: rowNum,
                    scrollOffset: 0,
                    loadComplete: function () {
                        var noHayRegistros = grid.getGridParam('records') == 0;
                        core.gridEmptyText(this, noHayRegistros)

                        if (noHayRegistros) {
                            core.hideGridEmptyPager(gridContainer);
                        }

                        if (func)
                            func();
                    }
                });

                var allowAdd = (gridMode == core.gridModes.Add || gridMode == core.gridModes.AddEdit);
                var allowEdit = (gridMode == core.gridModes.Edit || gridMode == core.gridModes.AddEdit);
                grid.jqGrid('navGrid', pager, { add: allowAdd, edit: allowEdit, del: false, refresh: false, search: false });
                return grid;
            },

            // Configuración inicial de grilla con subgrilla expandible
            // Parametros
            // gridContainer: Contenedor de la grilla
            // url: Url para la carga inicial de la grilla
            // colNames: Column Names en notación JSON
            // colModel: Column Model en notación JSON
            // sortName: Columna por la que se ordenará inicialmente
            // height: Alto que ocupará la grilla
            // rowNum: Cantidad de registros a mostrar por página
            // pager: Id del paginador de la grilla
            // gridMode: Valores posibles VIEW, EDIT
            // subGridRowExpandedFunction: Función ejecutada cada vez que se expande una fila de la grilla principal
            // readonly: Modo en que se mostrará la grilla
            getExpandibleGrid: function (gridContainer, url, colNames, colModel, sortName, height, rowNum, pager, gridMode, subGridRowExpandedFunction, readonly) {
                var grid = $(gridContainer);
                grid.jqGrid({
                    url: url,
                    datatype: 'json',
                    colNames: colNames,
                    colModel: colModel,
                    height: height,
                    loadonce: true,
                    gridview: true,
                    sortable: true,
                    sortname: sortName,
                    sortorder: 'asc',
                    multiselect: false,
                    pager: pager,
                    rowNum: rowNum,
                    scrollOffset: 0,
                    loadComplete: function () {
                        if (grid.getGridParam('records') == 0) GridEmptyText(this, true);
                        else core.gridEmptyText(this, false);
                    },
                    subGrid: true,
                    subGridOptions: {
                        "plusicon": "ui-icon-triangle-1-e",
                        "minusicon": "ui-icon-triangle-1-s",
                        "openicon": "ui-icon-arrowreturn-1-e",
                        "reloadOnExpand": false,
                        "selectOnExpand": true
                    },
                    subGridRowExpanded: function (subgridDivId, rowId) {
                        subGridRowExpandedFunction(subgridDivId, rowId, readonly);
                    }
                });

                var allowAdd = (gridMode == core.gridModes.Add || gridMode == core.gridModes.AddEdit);
                var allowEdit = (gridMode == core.gridModes.Edit || gridMode == core.gridModes.AddEdit);

                grid.jqGrid('navGrid', pager, { add: allowAdd, edit: allowEdit, del: false, refresh: false, search: false });

                return grid;
            },

            // Configuración inicial de grilla con totales
            // Parametros
            // gridContainer: Contenedor de la grilla
            // url: Url para la carga inicial de la grilla
            // colNames: Column Names en notación JSON
            // colModel: Column Model en notación JSON
            // sortName: Columna por la que se ordenará inicialmente
            // height: Alto que ocupará la grilla
            // rowNum: Cantidad de registros a mostrar por página
            // pager: Id del paginador de la grilla
            // gridMode: Valores posibles VIEW, EDIT
            // arrayColTotals: array de columnas de totales
            getTotalsGrid: function (gridContainer, url, colNames, colModel, sortName, height, rowNum, pager, gridMode, viewrecords, arrayColTotals){
                var grid = $(gridContainer);
                viewrecords = (viewrecords) ? viewrecords : false;
                grid.jqGrid({
                    url: url,
                    datatype: 'json',
                    colNames: colNames,
                    colModel: colModel,
                    height: height,
                    loadonce: true,
                    gridview: true,
                    sortable: true,
                    autowidth: true,
                    sortname: sortName,
                    sortorder: 'asc',
                    multiselect: false,
                    viewrecords: viewrecords,
                    pager: pager,
                    rowNum: rowNum,
                    scrollOffset: 0,
                    footerrow : true,
                    loadComplete: function (data) {
                        if (grid.getGridParam('records') == 0) core.gridEmptyText(this, true);
                        else core.gridEmptyText(this, false);

                        if (data && data.totals) {
                            var gridName = gridContainer.replace('#', '');
                            $('#gbox_' + gridName).find('.footrow [role=gridcell]').first().html('Total');
                            var i = 0;
                            $.each(arrayColTotals, function (key, colName) {
                                //var sum = grid.jqGrid('getCol', colName, false, 'sum');
                                $('#gbox_' + gridName).find('.footrow [aria-describedby=' + gridName + '_' + colName + ']').html(data.totals[i]);
                                i++;
                            });
                        }
                    }
                });

                var allowAdd = (gridMode == core.gridModes.Add || gridMode == core.gridModes.AddEdit);
                var allowEdit = (gridMode == core.gridModes.Edit || gridMode == core.gridModes.AddEdit);

                grid.jqGrid('navGrid', pager, { add: allowAdd, edit: allowEdit, del: false, refresh: false, search: false });

                return grid;
            },

            //Muestra el texto en las grillas cuando no se obtuvieron resultados
            gridEmptyText :function (grid, display) {
                //var emptyText = "No se encontraron resultados"; 
                //var container = $(grid).parents('.ui-jqgrid-view');
                //if (container.length > 1) container = $(container[0]);
                //if (display) {
                //    if(container.find('#emptyDataText').length==0) container.find('.ui-jqgrid-hdiv').after('<div id="emptyDataText" >' + emptyText + '</div>'); 
                //}
                //else {
                //    container.find('#emptyDataText').remove(); 
                //}
            },

            //Si tiene, oculta los controles de navegacion de paginas de la grilla en caso de que no se retornen registros:
            hideGridEmptyPager: function (grid, display){
                var pager = $(grid).jqGrid('getGridParam', 'pager');
                if (pager) {
                    core.setVisibilityControl(pager + '_center', display);
                }
            },

            // Agrega funcionalidad al botón Nuevo de la grilla
            // Parametros
            // gridContainer: Contenedor de la grilla
            // addFunction: Función a ejecutar cuando se presione el botón
            setButtonAddFunction: function (gridContainer, addFunction) {
                var addButtonId = gridContainer.replace('#', '#add_');
                $(addButtonId).click(function (event) {
                    addFunction();
                });
            },

            // Agrega funcionalidad al botón Editar de la grilla
            // Parametros
            // gridContainer: Contenedor de la grilla
            // editFunction: Función a ejecutar cuando se presione el botón
            setButtonEditFunction: function (gridContainer, editFunction) {
                var grid = $(gridContainer);
                var editButtonId = gridContainer.replace('#', '#edit_');
                $(editButtonId).click(function (event) {
                    var selr = core.isSelectedRow(gridContainer);
                    if (selr) {
                        editFunction(selr);
                    }
                });
            },

            // Agrega el botón Eliminar y su funcionalidad
            // Parametros
            // gridContainer: Contenedor de la grilla
            // pager: Paginador donde se situará el botón
            // deleteFunction: Función a ejecutar cuando se presione el botón
            setButtonDeleteFunction: function (gridContainer, pager, deleteFunction) {
                var grid = $(gridContainer);
                var deleteButtonId = gridContainer.replace('#', 'delete_');
                grid.jqGrid('navButtonAdd', pager, {
                    id: deleteButtonId, caption: '', buttonicon: 'ui-icon-trash', title: 'Eliminar fila seleccionada.',
                    onClickButton: function () {
                        var selr = core.isSelectedRow(gridContainer);
                        if (selr) {
                            core.showPopUpMessageConfirmation('Confirme', '¿Desea eliminar el registro seleccionado?', function () { deleteFunction(selr); });
                        }
                    }
                });
            },

            // Agrega un botón personalizado a la grilla
            // Parametros
            // gridContainer: Contenedor de la grilla
            // pager: Paginador donde se situará el nuevo botón
            // buttonId: Id a asignar el nuevo botón
            // buttonIcon: Icono del nuevo botón
            // buttonTooltipo: Tooltip que se visualizará al pasar el mouse sobre el botón
            // buttonFunction: Función a ejecutar cuando se presione el botón
            addCustomButton: function (gridContainer, pager, buttonId, buttonIcon, buttonTooltip, buttonFunction) {
                $(gridContainer).jqGrid('navButtonAdd', pager, {
                    id: buttonId, caption: '', buttonicon: buttonIcon, title: buttonTooltip,
                    onClickButton: function () {
                        buttonFunction();
                    }
                });
            },

            // Recarga la grilla
            // Parametros
            // gridContainer: Contenedor de la grilla
            // url: Url para recargar la grilla
            reloadGrid: function (gridContainer, url, func) {
                var grid = $(gridContainer);
                var currentPage = grid.jqGrid('getGridParam', 'page');
                grid.jqGrid('setGridParam', {
                    datatype: 'json',
                    url: url,
                    loadComplete: function (data) {
                        if (this.p.datatype === 'json') {
                            setTimeout(function () {
                                grid.trigger('reloadGrid', [{
                                    page: currentPage
                                }]);
                            });
                        };
                        if (grid.getGridParam('records') == 0) {
                            core.gridEmptyText(this, true);
                            core.hideGridEmptyPager(gridContainer, false);
                        }
                        else {
                            core.gridEmptyText(this, false);
                            core.hideGridEmptyPager(gridContainer, true);
                        }
                    },
                    loadError: function (xhr, status, error) {
                        debugger;
                    },
                    gridComplete: function () {
                        var _rows = $(".jqgrow");
                        for (var i = 0; i < _rows.length; i++) {
                            _rows[i].attributes["class"].value += " " + _rows[i].childNodes[0].textContent;
                        }
                        if (func!= null)
                            func();
                    }
                }).trigger('reloadGrid');
            },

            // Recarga la grilla y recalcula los totales
            // Parametros
            // gridContainer: Contenedor de la grilla
            // url: Url para recargar la grilla
            // arrayColTotals: array de columnas de totales
            reloadTotalsGrid: function (gridContainer, url, arrayColTotals) {
                var grid = $(gridContainer);
                var currentPage = grid.jqGrid('getGridParam', 'page');
                grid.jqGrid('setGridParam', {
                    datatype: 'json',
                    url: url,
                    loadComplete: function (data) {
                        if (this.p.datatype === 'json') {
                            setTimeout(function () {
                                grid.trigger('reloadGrid', [{
                                    page: currentPage
                                }]);
                            });
                        };
                        if (grid.getGridParam('records') == 0) core.gridEmptyText(this, true);
                        else GridEmptyText(this, false);

                        if (data.totals) {
                            var gridName = gridContainer.replace('#', '');
                            $('#gbox_' + gridName).find('.footrow [role=gridcell]').first().html('Total');
                            var i = 0;
                            $.each(arrayColTotals, function (key, colName) {
                                //var sum = grid.jqGrid('getCol', colName, false, 'sum');
                                $('#gbox_' + gridName).find('.footrow [aria-describedby=' + gridName + '_' + colName + ']').html(data.totals[i]);
                                i++;
                            });
                        }
                    },
                    loadError: function (xhr, status, error) {
                        debugger;
                    },
                    gridComplete: function () {
                        var _rows = $(".jqgrow");
                        for (var i = 0; i < _rows.length; i++) {
                            _rows[i].attributes["class"].value += " " + _rows[i].childNodes[0].textContent;
                        }
                    }
                }).trigger('reloadGrid');
            },

            // Obtiene el valor de una celda
            // Parámetros
            // gridContainer: Contenedor de la grilla
            // cellName: Nombre de la celda
            getCellValue: function(gridContainer, selr, cellName){
                return $(gridContainer).jqGrid('getCell', selr, cellName);
            },

            // Obtiene la sumatoria de una columna de una grilla
            // Parametros:
            // gridContainer: Contenedor de la grilla
            // cellName: Nombre de la celda
            getCellSum: function(gridContainer, cellName){
                return $(gridContainer).jqGrid('getCol', cellName, false, 'sum');
            },

            // Setea la cabecera donde se visualizará la fila seleccionada
            // Parametros
            // gridContainer: Contenedor de la grilla
            // headerContainer: Contenedor de la cabecera
            setViewHeader: function(gridContainer, headerContainer, onSelectRowFunction){
                var grid = $(gridContainer);
                var header = $(headerContainer);
                grid.jqGrid('setGridParam', {
                    onSelectRow: function (selr) {
                        var colModel = grid.jqGrid('getGridParam', 'colModel');
                        $.each(colModel, function (index, column) {
                            var headField = header.find('[id *= ' + column.name + 'Header]');
                            if (headField.length == 0) headField = header.find('#' + column.name);
                            if (headField.length != 0) {
                                var headValue;
                                if (column.type == 'bool') {
                                    if (eval(grid.jqGrid('getCell', selr, column.name)))
                                        headValue = column.trueVal;
                                    else
                                        headValue = column.falseVal;
                                }
                                else headValue = grid.jqGrid('getCell', selr, column.name);
                                headField.val(headValue);
                                if (headField.attr('data-val-number')) headField.val(headValue.replace('.',','));
                                if (headField.is('select') && headField.val() == null) {
                                    $("#" + column.name + " option:contains(" + headValue + ")").each(function () {
                                        if ($.trim($(this).text()) == headValue) {
                                            $(this).attr('selected', 'selected');
                                        }
                                    });
                                    if (headField.val() == null) {
                                        headField.append('<option value="' + headValue + '">' + headValue + '</option>');
                                        headField.val(headValue);
                                    }
                                }
                            };
                        });
                        if (onSelectRowFunction) onSelectRowFunction(gridContainer, selr);
                    }
                })
            },

            // Devuelve el id seleccionado en la grilla, caso contrario muestra un mensaje de error
            // Parametros
            // gridContainer: Contenedor de la grilla
            isSelectedRow: function (gridContainer) {
                var grid = $(gridContainer);
                var selr = grid.jqGrid('getGridParam', 'selrow');
                if (!selr) {
                    core.showPopUpMessageInformation('Selección de registro', 'No se ha seleccionado ningún registro.');
                }
                return selr;
            },

            // Configura un datepicker
            // Parametros 
            // datePickerId: Identificador del input donde se agregará el datepicker, si es null se configuran todos los controles que tengan la clase .settingdatepicker
            // dateFormat: Formato de la fecha a mostrar en el datepicker, el valor por defecto es dd/mm/yy
            configureDatePicker: function (container, datePickerId, dateFormat, onChangeFunction) {
                dateFormat = !dateFormat ? 'dd/mm/yy' : dateFormat;
                var target = !datePickerId ? container + ' .settingdatepicker' : container + ' ' + datePickerId;
                $(target).attr('readonly', true);
                    
                $(target).datepicker({
                    showOn: 'button',
                    buttonImage: '../Images/calendar.gif',
                    buttonImageOnly: true,
                    buttonText: 'Seleccione fecha',
                    changeMonth: true,
                    changeYear: true,
                    onChangeMonthYear: (dateFormat == 'yy/mm' || dateFormat == 'yy') ?
                        function (year, month, inst) {
                            if (dateFormat == 'yy') {
                                var date = new Date(year, 1);
                                $(this).datepicker("setDate", date);
                                if (onChangeFunction) onChangeFunction(year, month, inst)
                            }
                            else {
                                var date = new Date(year, month - 1, 1);
                                $(this).datepicker("setDate", date);
                                if (onChangeFunction) onChangeFunction(year, month, inst)
                            }
                        }: null,
                    dateFormat: dateFormat,
					//Fuerzo a que se muestre el calendar justo debajo del input
                    beforeShow: function (input, inst) {
                        var calendar = inst.dpDiv;
                        setTimeout(function () {
                            calendar.position({
                                my: 'right top',
                                at: 'right bottom',
                                collision: 'none',
                                of: input
                            });
                        }, 1);
                    }
                    
                });
				
                //Permito que se borre el contenido del target al hacer click
                $(target).click(function () { $(target).val(''); });

                //cuando se selecciona solo años
                if (dateFormat == 'yy') {
                    $(target).datepicker('option', 'stepMonths', 12);
                    $(target).datepicker('option', 'monthNames', ["", "", "", "", "", "", "", "", "", "", "", ""]);
                    $(target).datepicker('option', 'changeMonth', false);
                }

                //Cuando se selecciona la imagen del datepicker se muestra la fecha actual, si no se ha seleccionado una fecha previamente
                $(container + ' .ui-datepicker-trigger').click(function () {
                        if (!$(target).datepicker("getDate")) {
                            var today = new Date();
                            if (dateFormat == 'yy') {
                                todayString = today.getFullYear();
                            } else if (dateFormat == 'yy/mm') {
                                //Le sumo uno al mes porque javascript toma enero como 0
                                todayString = today.getFullYear() + '/' + (today.getMonth() + 1);
                                //$(target).datepicker('setDate', new Date(today.getFullYear(), today.getMonth()));
                            } else {
                                todayString = today.getDay() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
                            }
                            $(target).val(todayString)
                        }
                    })
				
                if (dateFormat == 'yy/mm' || dateFormat == 'yy') {
                    $(target).focus(function () {
                        $('.ui-datepicker-calendar').hide()
                    });
                }
            },

            // Muestra el dialog
            // Parametros            
            // container: Id del contenedor donde se va alojar el dialog.
            // titleDialog: Titulo del dialog.
            // width: Ancho del dialog.
            // minHeigth: Minimo de alto del dialog.
            showDialog: function (container, titleDialog, width, minHeigth) {
                var customDialog = $(container);
                customDialog.dialog({
                    title: titleDialog,
                    position: { my: 'center', at: 'center', of: window },
                    width: width,
                    height: 'auto',
                    minHeight: minHeigth,
                    modal: true,
                    resizable: false,
                    draggable: false,
                    create: function (event, ui) {
                        var widget = $(this).dialog('widget');
                        window.setTimeout(function () {
                            $(".ui-button .ui-button-icon-primary", widget)
                                .removeClass("ui-button-icon-primary ui-icon ui-icon-closethick")
                                .addClass("fontawesome fa fa-times-circle");
                        }, 0);
                    }


                })
            },
            
            // Carga una página en un dialog
            // url: URL del action que genera la página que va a contener el dialog.
            // container: Id del contenedor donde se alojar el tab por default.
            // data: Datos a enviar requeridos para el proceso de la página.
            // readonly: Indica si la página será de Consulta o de Edición
            // successFunction: Función que se ejecutará si la llamada fue exitosa
            loadDialogPage: function(url, dialogContainer, container, datos, readonly, successFunction) {
                var customDialog = $(dialogContainer);
                $.ajax({
                    cache: false,
                    type: 'GET',
                    url: url,
                    dataType: 'html',
                    data: datos,
                    success: function (data, textStatus) {
                        var customDialog = $(dialogContainer);
                        customDialog.find(container).html(data);
                        if (successFunction) successFunction();
                        {
                            core.setEnabledControls(container, !readonly);
                        }
                        //agrego los estilos y las funcionalidades necesarias
                        queryLoader.loadStylesReload();
                        //cambio los estilos de los botones
                        queryLoader.changeImageButtons();
                    },
                    error: function (xhr, err, e) {
                        customDialog.dialog('close');
                        core.showPopUpMessageError('Error!', 'Se produjo un error al cargar la página');
                    }
                })
            },

            // Carga una pagina dentro de otra.
            // url: URL del action que genera la página que va a contener el dialog.
            // container: Id del contenedor donde se va alojar.
            // datos: Datos a enviar requeridos para el proceso de la página.
            loadPage: function(url, container, datos) {
                var pageContainer = $(container);
                $.ajax({
                    cache: false,
                    type: 'GET',
                    url: url,
                    dataType: 'html',
                    data: datos,
                    async: false,
                    success: function (data, textStatus) {
                        pageContainer.html(data);
                    },
                    error: function (xhr, err, e) {
                        core.showPopUpMessageError('Error!', 'Se produjo un error al cargar la página');
                    }
                })
            },

            // Realiza una llamada ajax
            // Parametros:
            // url: URL del action
            // data: Datos a enviar requeridos para la llamada
            // successFunction: Función que se ejecutará si la llamada fue exitosa
            executeAjax: function(url, data, dataType, successFunction){
                $.ajax({
                    type: 'GET',
                    cache: false,
                    url: url,
                    dataType: dataType,
                    data: data,
                    success: function (data, textStatus) {
                        if (successFunction) successFunction(data,textStatus);
                    },
                    error: function (xhr, err, e) {
                        core.showPopUpMessageError('Error!', 'Se produjo un error al realizar la acción solicitada');
                    }
                })
            },

            // Realiza una llamada ajax de tipo POST
            // Parametros:
            // url: URL del action
            // data: Datos a enviar requeridos para la llamada
            // successFunction: Función que se ejecutará si la llamada fue exitosa
            executeAjaxPost: function (url, data, dataType, successFunction) {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
                    dataType: dataType,
                    data: data,
                    success: function (data, textStatus) {
                        if (successFunction) successFunction(data, textStatus);
                    },
                    error: function (xhr, err, e) {
                        core.showPopUpMessageError('Error!', 'Se produjo un error al realizar la acción solicitada');
                    }
                })
            },

            // Obtiene los permisos por funcionalidad.
            // Parametros:
            // IdFuncionalidad: Identificador de la funcionalidad.
            getPermisos: function (idFuncionalidad) {
                var result;
                $.ajax({
                    cache: false,
                    async: false,
                    type: 'GET',
                    url: '/Infrastructure/GetPermisos?idFuncionalidad=' + idFuncionalidad,
                    dataType: 'json',
                    success: function (data, textStatus) {
                        result = data;
                    },
                    error: function (xhr, err, e) {
                        core.showPopUpMessageError('Error!', 'Se produjo un error al realizar la acción solicitada');
                    }
                });
                return result;
            },

            // Habilita o deshabilita todos los controles contenidos en container, también oculta mensajes de validación
            // Parametros:
            // container: contenedor de los controles
            // enable: true->habilita, false->deshabilita
            setEnabledControls: function (container, enable) {
                $(container + ' :input').attr('disabled', !enable);                 // Todos los inputs
                $(container + ' a').attr('readonly', !enable);                      // Todos los anchors
                $(container + ' span').attr('readonly', !enable);                   // Todos los span
                if (enable) {
                    $(container + ' a').removeClass('campodisabled');
                    $(container + ' a').removeAttr('readonly');
                }
                else
                    $(container + ' a').addClass('campodisabled');

                $(container + ' .ui-datepicker-trigger').attr('disabled', !enable); // Todos los datepickers
            },

            // Muestra u oculta un control
            // Parametros:
            // controlId: Id del control
            // visible: true->visible, false->oculto
            setVisibilityControl: function (controlId, visible){
                //if (visible) $(controlId).removeClass('ui-helper-hidden');
                //else $(controlId).addClass('ui-helper-hidden');
                if (visible) $(controlId).show();
                else $(controlId).hide();
            },
                
            // Resetea todos los controles contenidos en container
            // Parametros:
            // container: contenedor de los controles
            // resetValidations: indica si se ocultan las validaciones
            resetFields: function (container, resetValidations) {
                $(container + ' :input[type!=submit]').val(null);
                if (resetValidations) $(container + ' :input').removeClass('input-validation-error');     // Todos los mensajes de validacion
                if (resetValidations) HideMensajeError(container);
            },

            // Formatea una fecha
            // Parametros:
            // date: fecha
            // devuelve: fecha en formato francés
            formatDate: function (date) {
                if (date) {
                    var partes = date.split('/')
                    return partes[1] + '/' + partes[0] + '/' + partes[2];
                }
                return null;
                //if (date) {
                //    var fecha = new Date(date);
                //    return fecha.getDate() + '/' + (fecha.getMonth() + 1) + '/' + fecha.getFullYear()
                //} else {
                //    return null
                //}
            },

            // Formatea el valor de un datePicker para pasarlo como parametro GET
            // Parametros:
            // datePickerValue: valor datePicker
            // devuelve: fecha en formato inglés
            parseDatePickerToParam: function (datePickerValue) {
                return $.datepicker.formatDate('mm/dd/yy', $.datepicker.parseDate('dd/mm/yy', datePickerValue))
            },

            //Oculta el mensaje de error
            hideMensajeError : function (container) {
                $(container).find("#mensajeError").slideUp();
            },

            //Muestra el mensaje de error
            showMensajeError : function (errors, container) {
                $(container).find('#errorsValidation')[0].innerHTML = errors;
                $(container).find("#mensajeError").slideDown();
            },

            // Funcion que maneja el onclick de los anchor para evitar scrolling.
            onClickAnchor : function (event, acceptCallback) {
                if (event.preventDefault) event.preventDefault();
                else event.returnValue = false;
                if ($(event.srcElement).attr("readonly") != 'readonly') acceptCallback();
            },

            // Popup Informativo.
            showPopUpMessageInformation : function(title, description, acceptCallback) {
                $("#popupMessageDescription")[0].innerHTML = description;
                $("#popupMessageIcon").addClass("awesome icon-info icon-large");
                $("#popupMessage").dialog({
                    title: title,
                    modal: true,
                    buttons: [
                      {
                          text: "Aceptar",
                          click: function () {
                              if (acceptCallback)
                                  acceptCallback();
                              $(this).dialog("close");
                          }
                      }
                    ],
                    closeOnEscape: false,
                    create: function (event, ui) {
                        $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                    }
                });
            },

            // Popup Confirmación
            showPopUpMessageConfirmation : function (title, description, acceptCallback, cancelCallback) {
                $("#popupMessageDescription")[0].innerHTML = description;
                $("#popupMessageIcon").addClass("fontawesome icon-info icon-large");
                $("#popupMessage").dialog({
                    title: title,
                    modal: true,
                    buttons: [
                      {
                          text: "Aceptar",
                          click: function () {
                              if (acceptCallback)
                                  acceptCallback();
                              $(this).dialog("close");
                          }
              
                      },
                      {
                          text: "Cancelar",
                          click: function () {
                              if (cancelCallback)
                                  cancelCallback();
                              $(this).dialog("close");
                          }
                      }
                    ],
                    closeOnEscape: false,
                    create: function (event, ui) {
                        $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                    }
                });
            },

            // Popup Error.
            showPopUpMessageError : function(title, description, acceptCallback) {
                $("#popupMessageDescription")[0].innerHTML = description;
                $("#popupMessageIcon").addClass("fontawesome icon-remove icon-large");
                $("#popupMessage").dialog({
                    title: title,
                    modal: true,
                    buttons: [
                      {
                          text: "Aceptar",
                          click: function () {
                              if (acceptCallback)
                                  acceptCallback();
                              $(this).dialog("close");
                          }
                      }
                    ],
                    closeOnEscape: false,
                    create: function (event, ui) {
                        $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                    }
                });
            },

            // Obtiene los permisos por funcionalidad.
            // Parametros:
            // IdFuncionalidad: Identificador de la funcionalidad.
            getPermisos: function (idFuncionalidad) {
            var result;
            $.ajax({
                cache: false,
                async: false,
                type: 'GET',
                url: '/Infrastructure/GetPermisos?funcionalidadId=' + idFuncionalidad,
                dataType: 'json',
                success: function (data, textStatus) {
                    result = data;
                },
                error: function (xhr, err, e) {
                    ShowPopUpMessageError('Error!', 'Se produjo un error al realizar la acción solicitada');
                }
            });
            return result;
            },
            formatosValidacion: {Enteros: 0, Naturales: 1, Reales: 2, RealesPositivos: 3},
            getValidador: function (formato) {

                function invalidarEvento(event) {
                    if (event.preventDefault) {
                        event.preventDefault();
                    }
                    else {
                        event.returnValue = false;
                    }
                }

                function getCodigo(event) {
                    event = event || window.event;
                    var codigo = event.keyCode ? event.keyCode : event.charCode;
                    return codigo;
                }
                
                function validaNumerosReales(event) {
                    var codigo = getCodigo(codigo);
                    //Conjunto de caracteres validos
                    if (validaAcciones(codigo) ||
                            (codigo >= 48 && codigo <= 57) || //0..9
                            (codigo >= 96 && codigo <= 105) || //numpad 0 .. numpad 9
                            codigo == 109 || //subtract
                            codigo == 110 || //decimal point
                            codigo == 188 || //comma
                            codigo == 189 || //dash
                            codigo == 190 //punto
                       ) { return true; }
                    invalidarEvento(event);
                }

                function validaNumerosRealesPositivos(event) {
                    var codigo = getCodigo(codigo);
                    //Conjunto de caracteres validos
                    if (validaAcciones(codigo) ||
                            (codigo >= 48 && codigo <= 57) || //0..9
                            (codigo >= 96 && codigo <= 105) || //numpad 0 .. numpad 9
                            codigo == 110 || //decimal point
                            codigo == 188 || //comma
                            codigo == 190 //punto
                       ) { return true; }
                    invalidarEvento(event);
                }

                function validaAcciones(codigo) {
                    if (codigo == 8 ||  //backspace
                            codigo == 9 ||  //tab
                            codigo == 13 || //enter
                            (codigo >= 16 && codigo <= 18) || //shift, ctrl, alt
                            (codigo >= 35 && codigo <= 40) || //end, home, left arrow, up arrow, right arrow, down arrow
                            codigo == 46 //delete
                       ) { return true; }

                    return false;
                }

                function validaNumerosEnteros(event) {
                    var codigo = getCodigo(codigo);

                    //Conjunto de caracteres validos
                    if (validaAcciones(codigo) ||
                            (codigo >= 48 && codigo <= 57) || //0..9
                            (codigo >= 96 && codigo <= 105) || //numpad 0 .. numpad 9
                            codigo == 109 || //subtract
                            codigo == 189 //dash
                       ) { return true; }

                    invalidarEvento(event)
                }

                function validaNumerosNaturales(event) {
                    var codigo = getCodigo(codigo);

                    //Conjunto de caracteres validos
                    if (validaAcciones(codigo) ||
                            (codigo >= 48 && codigo <= 57) || //0..9
                            (codigo >= 96 && codigo <= 105) //numpad 0 .. numpad 9
                       ) { return true; }

                    invalidarEvento(event)
                }
                
                if (formato == core.formatosValidacion.Enteros) {
                    return validaNumerosEnteros;
                }
                if (formato == core.formatosValidacion.Naturales) {
                    return validaNumerosNaturales;
                }
                if (formato == core.formatosValidacion.Reales) {
                    return validaNumerosReales;
                }
                if (formato == core.formatosValidacion.RealesPositivos) {
                    return validaNumerosRealesPositivos;
                }
            },

            //Pone el foco en el control indicado
            //Al inyectar el html de una partial view dentro de un dialog muchas veces tarda demasiado y no ponia el foco.
            setFocus: function (control) {
                setTimeout(function () {
                    if ($(control)) {
                        $(control).focus();
                    }
                }, 250);
            },

            //Carga el dropDownList con los valores retornados por el metodo especificado por 'url'.
            //Parametros:
            //ddl: dropDownList a cargar
            //url: url usada para cargar los datos al ddl
            llenarDropDownList: function (ddl, url, urlParameters,textField, dataField, defaultText, defaultValue, func) {
                core.executeAjaxPost(url, urlParameters, 'json', function (data) {
                    var select = $(ddl).empty();

                    if (defaultText)
                        select.append('<option value="' + defaultValue ? defaultValue : '' + '">' + defaultText + '</option>');

                    $.each(data, function (i, item) {

                        select.append('<option value="'
                                             + item[dataField]
                                             + '">'
                                             + item[textField]
                                             + '</option>');
                    });
                    if (func)
                        func();
                });
            },
            //Asocia un dropdownlist a otro.
            //Parametros:
            //ddl: dropdownlist dependiente.
            //ddlPadre: dropdownlist del que depende.
            //urlDatos: url usada para cargar los datos al ddl
            //valorDefaultPadre: es el valor del item default del padre
            //func: funcion a ejecutar luego de cargar el ddl hijo
            asociarDropDownList: function (ddl, ddlPadre, urlDatos, valorDefaultPadre, urlParameter, textField, dataField, defaultText, defaultValue, func) {
                //$(ddl).attr("disabled", "disabled");
                $(ddlPadre).change(function () {
                    var valorPadre = $(ddlPadre).val();
                    if (valorPadre != valorDefaultPadre)
                    {
                        //$(ddl).removeAttr("disabled");
                        var parametros = {};
                        parametros[urlParameter] = valorPadre;
                        core.llenarDropDownList(ddl, urlDatos, parametros, textField, dataField, defaultText, defaultValue, func)
                    } else {
                        //$(ddl).attr("disabled", "disabled");
                        $(ddl).empty();
                    }
                });
            },

            //Retorna el contenido del datepicker sumado a la cantidad de dias indicadas en el parametro
            addDays: function (textBox, dias) {
                var date = $(textBox).datepicker("getDate", date);
               
                if (date) {
                    date = new Date(date.getYear(), date.getMonth(), date.getDate() + parseFloat(dias));
                }
                return date;
            },

            //Agrega una funcion para que se ejecute cuando cambie el contenido de un textbox:
            setChangeTextBox: function (textBox, funcion) {
                $(textBox).blur(funcion)
                .focusout(funcion)
                .keypress(funcion)
                .keyup(funcion)
            },
            //Agrega una funcion para que se ejecute cuando cambie el contenido de un textbox:
            setTextBoxEvents: function (textBox, eventos, funcion) {
                if (eventos) {
                    for (var i = 0; i < eventos.length; i++) {
                        $(textBox).trigger(eventos[i], funcion);
                    }
                } 
            },

            //Blanquea los textbox y pone el valor default de los dropdownlist
            limpiarControles: function (contenedor) {
                $(contenedor + ' :input[type=text]').val('');
                $('input[type=checkbox]').prop("checked", false);
            },

            //str: es un string en formato numerico con '.' como delimitador de decimales
            //cantidadDecimales: cantidad de decimales que se quiere que tenga el numero de salida (puede ser nulo)
            //Retorna el string de entrada pero con ',' como delimitador de decimales.
            addCommas: function (str, cantidadDecimales) {
                //Redondeo el numero:
                if (cantidadDecimales) {
                    var multiplicador = 1;
                    for (var i = 0; i < cantidadDecimales; i++)
                        multiplicador = multiplicador * 10;

                    str = Math.round(str * multiplicador) / multiplicador;
                }

                var parts = (str + "").split("."),
                    main = parts[0],
                    len = main.length,
                    output = "",
                    i = len - 1;

                while (i >= 0) {
                    output = main.charAt(i) + output;
                    if ((len - i) % 3 === 0 && i > 0) {
                        output = "" + output;
                        //output = "." + output;
                    }
                    --i;
                }
                // put decimal part back
                if (parts.length > 1) {
                    largo = parts[1].length > cantidadDecimales ? cantidadDecimales : parts[1].length;
                    output += "," + (parts[1].substr(0, largo));
                }
                return output;
            },

            //Retorna el valor del textbox como numero
            getNumber: function (textBox) {
                var numStr = $(textBox).val().replace(",", ".");
                return parseFloat(numStr);
            },

            getNumberFromString: function (str) {
                var num1 = str.replace(".", "");
                var numStr = num1.replace(",", ".");
                return parseFloat(numStr);
            },

            //Formateador de numeros jqgrid:
             numFormat: function (cellvalue, options, rowObject) {
                var numero = cellvalue || cellvalue == '0' ? cellvalue.toString().replace(".", ",") : "";
                
                var numResul = '';
                for (var i = numero.length - 1, j = 0; i >=0; i--) {
                    if (numero[i] != '-') {
                        if (numero[i] == ',') {
                            j = 0;
                        }else if (j == 3) {
                            numResul = '.' + numResul;
                            j = 1;
                        } else {   
                            j++;
                        }
                    }
                    numResul = numero[i] + numResul;
                }
                return numResul;
             },

             numFormatOrNa: function (cellvalue, options, rowObject) {
                 if (cellvalue == null) {
                     return "NA";
                 }

                 var numero = cellvalue || cellvalue == '0' ? cellvalue.toString().replace(".", ",") : "";

                 var numResul = '';
                 for (var i = numero.length - 1, j = 0; i >= 0; i--) {
                     if (numero[i] != '-') {
                         if (numero[i] == ',') {
                             j = 0;
                         } else if (j == 3) {
                             numResul = '.' + numResul;
                             j = 1;
                         } else {
                             j++;
                         }
                     }
                     numResul = numero[i] + numResul;
                 }
                 return numResul;
             },
        }
    }
    )();

