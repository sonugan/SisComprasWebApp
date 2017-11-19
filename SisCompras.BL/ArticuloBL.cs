using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using AccesoDatos;
using System.Data;
using Modelos.Dtos;

namespace SisCompras.BL
{
    public class ArticuloBL
    {
        public string CargarArticulos(CatalogoModel catalogoModel, string sArchivo)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            string l_s_Error = "";

            try
            {
                MonedaDAO l_dao_Moneda = new MonedaDAO();
                RubroDAO l_dao_Rubro = new RubroDAO();
                int iFila = 0;
                int iMoneda = 0;
                int iRubroId = 0;
                int iArticuloId = 0;

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + sArchivo, "ArticuloBL.cs", "CargarArticulos");

                //El proveedor es obligatorio
                if(catalogoModel.ProveedorId <= 0)
                {
                    return "Debe ingresar el proveedor";
                }

                //El código del artículo es obligatorio
                if (catalogoModel.Codigo < 0)
                {
                    return "Debe indicar la columna para el código";
                }

                //El nombre del artículo es obligatorio
                if (catalogoModel.Nombre < 0)
                {
                    return "Debe indicar la columna para el nombre";
                }

                //La moneda es obligatoria, pero puede venir como columna del archivo
                //o se pudo haber elegido en el DropDownList
                if (catalogoModel.MonedaId <= 0)
                {
                    if (catalogoModel.Moneda < 0)
                    {
                        return "Debe seleccionar una moneda o indicar la columna para la moneda";
                    }
                }

                ExcelImportBL excel = new ExcelImportBL();
                DataSet ds = excel.ImportExcelXLSFromFile(sArchivo, false);//lo mando como que no tiene títulos
                DataTable dt = ds.Tables[0];
                ArticuloModel articuloModel = new ArticuloModel();
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();

                //Recorro el dataTable y voy insertando cada artículo
                foreach (DataRow row in dt.Rows)
                {
                    iFila += 1;

                    //La moneda es obligatoria, pero puede venir como columna del archivo
                    //o se pudo haber elegido en el DropDownList
                    iMoneda = catalogoModel.MonedaId;
                    if (iMoneda <= 0)
                    {
                        if (row[catalogoModel.Moneda].ToString() != "")
                        {
                            iMoneda = l_dao_Moneda.ObtenerId(row[catalogoModel.Moneda].ToString());
                            if (iMoneda <= 0)
                            {
                                l_s_Mensaje = l_s_Mensaje + "La moneda " + row[catalogoModel.Moneda].ToString() + " en la fila " + iFila.ToString() + " no existe" + Environment.NewLine;
                            }
                        }
                    }

                    //Si se informa el rubro
                    if (catalogoModel.Rubro > 0)
                    {
                        if (row[catalogoModel.Rubro].ToString() != "")
                        {
                            iRubroId = l_dao_Rubro.ObtenerId(row[catalogoModel.Rubro].ToString());
                            if (iRubroId <= 0)
                            {
                                //Doy de alta el rubro
                                RubroModel rubro = new RubroModel();
                                rubro.Activo = "Si";
                                rubro.Codigo = null;
                                rubro.Descripcion = "Dado de alta desde importación";
                                rubro.Nombre = row[catalogoModel.Rubro].ToString();
                                rubro.LoginCreacion = catalogoModel.LoginCreacion;

                                l_s_Error = l_dao_Rubro.Insertar(rubro);
                                if (!Int32.TryParse(l_s_Error, out iRubroId)) //Hubo un error
                                {
                                    iRubroId = 0;
                                    l_s_Mensaje += "Error al intentar crear el rubro " + row[catalogoModel.Rubro].ToString() + Environment.NewLine;
                                }
                            }
                        }
                    }

                    articuloModel.Activo = "Si";
                    if (catalogoModel.Codigo >= 0) { articuloModel.Codigo = row[catalogoModel.Codigo].ToString(); }
                    if (catalogoModel.CodigoBarras >= 0) { articuloModel.CodigoBarras = row[catalogoModel.CodigoBarras].ToString(); }
                    if (catalogoModel.CBM >= 0) { articuloModel.CostoCBM = (row[catalogoModel.CBM].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.CBM].ToString()); }
                    if (catalogoModel.INNER >= 0) { articuloModel.CostoINNER = (row[catalogoModel.INNER].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.INNER].ToString()); }
                    if (catalogoModel.RMB >= 0) { articuloModel.CostoRMB = (row[catalogoModel.RMB].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.RMB].ToString()); }
                    if (catalogoModel.Cotizacion >= 0) { articuloModel.Cotizacion = (row[catalogoModel.Cotizacion].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.Cotizacion].ToString()); }
                    if (catalogoModel.Descripcion >= 0) { articuloModel.Descripcion = row[catalogoModel.Descripcion].ToString(); }
                    if (catalogoModel.Descripcion2 >= 0) { articuloModel.Descripcion2 = row[catalogoModel.Descripcion2].ToString(); }
                    articuloModel.FechaCreacion = DateTime.Now;
                    //articuloModel.FechaUltModif = null;
                    articuloModel.LoginCreacion = catalogoModel.LoginCreacion;
                    articuloModel.LoginUltModif = "";
                    articuloModel.MonedaId = iMoneda;
                    if (catalogoModel.Nombre >= 0) { articuloModel.Nombre = row[catalogoModel.Nombre].ToString(); }
                    if (catalogoModel.Observaciones1 >= 0) { articuloModel.Observaciones1 = row[catalogoModel.Observaciones1].ToString(); }
                    if (catalogoModel.Observaciones2 >= 0) { articuloModel.Observaciones2 = row[catalogoModel.Observaciones2].ToString(); }
                    if (catalogoModel.Observaciones3 >= 0) { articuloModel.Observaciones3 = row[catalogoModel.Observaciones3].ToString(); }
                    articuloModel.PendienteRecep = 0;
                    if (catalogoModel.PesoBruto >= 0) { articuloModel.PesoBruto = (row[catalogoModel.PesoBruto].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.PesoBruto].ToString()); }
                    if (catalogoModel.PesoNeto >= 0) { articuloModel.PesoNeto = (row[catalogoModel.PesoNeto].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.PesoNeto].ToString()); }
                    if (catalogoModel.Precio >= 0) { articuloModel.PrecioEnMoneda = (row[catalogoModel.Precio].ToString() == "") ? (decimal)0 : Convert.ToDecimal(row[catalogoModel.Precio].ToString()); }
                    articuloModel.ProveedorId = catalogoModel.ProveedorId;//Acá tengo directamente el proveedor, no una columna del excel
                    articuloModel.RubroId = iRubroId;
                    if (catalogoModel.UniXbulto >= 0) { articuloModel.UniXbulto = (row[catalogoModel.UniXbulto].ToString() == "") ? (Int16)0 : Convert.ToInt16(row[catalogoModel.UniXbulto].ToString()); }
                    articuloModel.Version = 1;

                    //Verifica si existe y actualiza o inserta. Puede insertar
                    //el rubro si no existe.
                    l_s_Error = l_dao_Articulo.Importar(articuloModel);
                    if (l_s_Error != "")
                    {
                        if (!Int32.TryParse(l_s_Error, out iArticuloId)) //Hubo un error
                        { 
                            l_s_Mensaje += l_s_Error + Environment.NewLine;
                        }
                    }
                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje += miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "CargarArticulos");
                return l_s_Mensaje;
            }
            finally { }
        }

        public ArticuloModel Consultar(int articuloId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + articuloId.ToString(), "ArticuloBL.cs", "Consultar");

                ArticuloModel model = new ArticuloModel();
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();

                model = l_dao_Articulo.Consultar(articuloId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public List<ArticuloModel> ConsultarArticulosCarga(string sProveedorId, string sFechaCargaDesde, string sFechaCargaHasta = "")
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();
                
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloBL.cs", "ConsultarArticulosCarga");
                
                return l_dao_Articulo.ConsultarArticulosCarga(sProveedorId, sFechaCargaDesde, sFechaCargaHasta); 

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "ConsultarArticulosCarga");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarArticulos()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();
                DataTable l_dt_Articulos = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloBL.cs", "ConsultarArticulos");

                l_dt_Articulos = l_dao_Articulo.ConsultarArticulos();
                return l_dt_Articulos;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "ConsultarArticulos");
                return null;
            }
            finally { }
        }

        public string Eliminar(int articuloId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + articuloId.ToString() + ", " + sUsuario, "ArticuloBL.cs", "Eliminar");

                ArticuloDAO l_dao_Articulo = new ArticuloDAO();
                l_s_Mensaje = l_dao_Articulo.Eliminar(articuloId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ArticuloModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();
                string l_s_Codigo = model.Codigo.ToUpper();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "ArticuloBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Articulo.BuscarCodigo(l_s_Codigo, model.ID, model.ProveedorId);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Articulo.BuscarNombre(model.Nombre, model.ID, model.ProveedorId);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Articulo.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(ArticuloModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ArticuloDAO l_dao_Articulo = new ArticuloDAO();
                MonedaDAO l_dao_Moneda = new MonedaDAO();
                string l_s_MonedaNacCod = "";
                int l_i_MonedaNacId = 0;

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.Codigo, "ArticuloBL.cs", "Insertar");

                model.Codigo = model.Codigo.ToUpper();
                l_s_Mensaje = l_dao_Articulo.BuscarCodigo(model.Codigo, 0, model.ProveedorId);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                model.Nombre = l_dao_Generales.InitCap(model.Nombre);
                l_s_Mensaje = l_dao_Articulo.BuscarNombre(model.Nombre, 0, model.ProveedorId);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Moneda.ObtenerMonedaNacional(out l_i_MonedaNacId, out l_s_MonedaNacCod);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }
                else
                {
                    //Si no se usó la moneda nacional, la cotización es obligatoria
                    if (l_i_MonedaNacId != model.MonedaId)
                    {
                        if (model.Cotizacion <= 0)
                        {
                            return "Debe ingresar la cotización.";
                        }
                    }
                    else
                    {
                        model.Cotizacion = 1;//si es la moneda nacional, enchufo 1 a la cotización
                    }
                }

                l_s_Mensaje = l_dao_Articulo.Insertar(model);

                return l_s_Mensaje;//devuelve el Id generado

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        //private IList<ArticuloModel> BaseSearch(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        //{
        //    var searchBy = (model.search != null) ? model.search.value : null;
        //    var take = model.length;
        //    var skip = model.start;

        //    string sortBy = "";
        //    bool sortDir = true;

        //    if (model.order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        sortBy = model.columns[model.order[0].column].data;
        //        sortDir = model.order[0].dir.ToLower() == "asc";
        //    }

        //    // search the dbase taking into consideration table sorting and paging
        //    return new List<ArticuloModel>();//GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
        //    //if (result == null)
        //    //{
        //    //    // empty collection...
        //    //    return new List<ArticuloModel>();
        //    //}
        //    //return result;
        //}
    }
}
