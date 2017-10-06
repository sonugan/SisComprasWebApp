using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using Modelos;

namespace AccesoDatos
{
    public class ArticuloDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public string BuscarNombre(string sNombre, int iId, int iProveedorId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT articulo_x_proveedor_id";
                l_s_stSql += " FROM articulos_x_proveedores";
                l_s_stSql += " WHERE UPPER(articulo_nombre) = '" + sNombre.ToUpper() + "'";
                l_s_stSql += " AND proveedor_id = " + iProveedorId.ToString();

                if (iId > 0)
                {
                    l_s_stSql += " AND articulo_x_proveedor_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ArticuloDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un artículo con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "BuscarNombre");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string BuscarCodigo(string sCodigo, int iId, int iProveedorId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT articulo_x_proveedor_id";
                l_s_stSql += " FROM articulos_x_proveedores";
                l_s_stSql += " WHERE articulo_cod = '" + sCodigo + "'";
                l_s_stSql += " AND proveedor_id = " + iProveedorId.ToString();

                if (iId > 0)
                {
                    l_s_stSql += " AND articulo_x_proveedor_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ArticuloDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un artículo con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ArticuloModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();//                                        1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0  1  2  3
                    string l_s_stSql = "{ CALL sp_articulosXproveedores_update (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Versión
                        OdbcParameter param = command.Parameters.Add("version", OdbcType.Int);
                        param.Value = model.Version;
                        //Id
                        param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = model.ID;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (model.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Codigo.ToUpper();
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(model.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (model.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = model.Activo;
                        //Precio
                        param = command.Parameters.Add("precio", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PrecioEnMoneda;
                        //Id de la moneda
                        param = command.Parameters.Add("moneda_id", OdbcType.Int);
                        param.Value = model.MonedaId;
                        //Id del proveedor
                        param = command.Parameters.Add("proveedor_id", OdbcType.Int);
                        param.Value = model.ProveedorId;
                        //Id del rubro
                        param = command.Parameters.Add("rubro_id", OdbcType.Int);
                        if (model.RubroId <= 0)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.RubroId;
                        }
                        //Descripción 2
                        param = command.Parameters.Add("descripcion2", OdbcType.VarChar, 250);
                        if (model.Descripcion2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion2;
                            }
                        }
                        //Código de barras
                        param = command.Parameters.Add("codigo_barras", OdbcType.VarChar, 50);
                        if (model.CodigoBarras == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.CodigoBarras == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.CodigoBarras;
                            }
                        }
                        //Costo RMB
                        param = command.Parameters.Add("costo_rmb", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoRMB;
                        //Cotización
                        param = command.Parameters.Add("cotizacion", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.Cotizacion;
                        //Unidades por bulto
                        param = command.Parameters.Add("uni_x_bulto", OdbcType.Int);
                        param.Value = model.UniXbulto;
                        //Costo INNER
                        param = command.Parameters.Add("costo_inner", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoINNER;
                        //Costo CBM
                        param = command.Parameters.Add("costo_cbm", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoCBM;
                        //Peso neto
                        param = command.Parameters.Add("peso_neto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoNeto;
                        //Peso bruto
                        param = command.Parameters.Add("peso_bruto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoBruto;
                        //Observaciones 1
                        param = command.Parameters.Add("obs1", OdbcType.VarChar, 50);
                        if (model.Observaciones1 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones1 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones1;
                            }
                        }
                        //Observaciones 2
                        param = command.Parameters.Add("obs2", OdbcType.VarChar, 250);
                        if (model.Observaciones2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones2;
                            }
                        }
                        //Observaciones 3
                        param = command.Parameters.Add("obs3", OdbcType.VarChar, 250);
                        if (model.Observaciones3 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones3 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones3;
                            }
                        }
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = model.LoginUltModif;

                        command.ExecuteNonQuery();
                        trx.Commit();
                    }

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public ArticuloModel Consultar(int iArticuloId)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iArticuloId.ToString(), "ArticuloDAO.cs", "Consultar");

                DataTable l_dt_Articulo = new DataTable();
                ArticuloModel model = new ArticuloModel();

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "SELECT articulo_x_proveedor_id, proveedor_id, proveedor_cod, proveedor_nombre,";
                    l_s_stSql += " articulo_cod, articulo_nombre, articulo_desc, flag_activo, numero_version,";
                    l_s_stSql += " moneda_id, moneda_cod, precio_en_moneda, pendiente_recepcion,";
                    l_s_stSql += " rubro_id, rubro_cod, rubro_nombre,";
                    l_s_stSql += " articulo_desc2, codigo_barras, costo_rmb,";
                    l_s_stSql += " cotizacion, unidades_x_bulto, costo_inner,";
                    l_s_stSql += " costo_cbm, peso_neto, peso_bruto,";
                    l_s_stSql += " observaciones_1, observaciones_2, observaciones_3";
                    l_s_stSql += " FROM vw_articulos";
                    l_s_stSql += " WHERE articulo_x_proveedor_id = ?";
                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ArticuloDAO.cs", "Consultar");

                    OdbcDataAdapter l_da_Articulo = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Articulo.SelectCommand.Parameters.AddWithValue("id", iArticuloId);
                    l_da_Articulo.Fill(l_dt_Articulo);

                }

                if (l_dt_Articulo.Rows.Count == 1)
                {
                    model.ID = Convert.ToInt32(l_dt_Articulo.Rows[0]["articulo_x_proveedor_id"].ToString());
                    model.Codigo = l_dt_Articulo.Rows[0]["articulo_cod"].ToString();
                    model.Nombre = l_dt_Articulo.Rows[0]["articulo_nombre"].ToString();
                    model.Descripcion = l_dt_Articulo.Rows[0]["articulo_desc"].ToString();
                    model.Activo = l_dt_Articulo.Rows[0]["flag_activo"].ToString();
                    model.Version = Convert.ToInt32(l_dt_Articulo.Rows[0]["numero_version"].ToString());
                    model.MonedaId = Convert.ToInt32(l_dt_Articulo.Rows[0]["numero_version"].ToString());
                    if (l_dt_Articulo.Rows[0]["precio_en_moneda"].ToString() != "") { model.PrecioEnMoneda = Convert.ToDecimal(l_dt_Articulo.Rows[0]["precio_en_moneda"].ToString()); }
                    model.ProveedorId = Convert.ToInt32(l_dt_Articulo.Rows[0]["proveedor_id"].ToString());
                    if (l_dt_Articulo.Rows[0]["cotizacion"].ToString() != "") { model.Cotizacion = Convert.ToDecimal(l_dt_Articulo.Rows[0]["cotizacion"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["costo_rmb"].ToString() != "") { model.CostoRMB = Convert.ToDecimal(l_dt_Articulo.Rows[0]["costo_rmb"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["costo_inner"].ToString() != "") { model.CostoINNER = Convert.ToDecimal(l_dt_Articulo.Rows[0]["costo_inner"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["costo_cbm"].ToString() != "") { model.CostoCBM = Convert.ToDecimal(l_dt_Articulo.Rows[0]["costo_cbm"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["unidades_x_bulto"].ToString() != "") { model.UniXbulto = Convert.ToInt16(l_dt_Articulo.Rows[0]["unidades_x_bulto"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["peso_bruto"].ToString() != "") { model.PesoBruto = Convert.ToDecimal(l_dt_Articulo.Rows[0]["peso_bruto"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["peso_neto"].ToString() != "") { model.PesoNeto = Convert.ToDecimal(l_dt_Articulo.Rows[0]["peso_neto"].ToString()); }
                    if (l_dt_Articulo.Rows[0]["rubro_id"].ToString() != "") { model.RubroId = Convert.ToInt32(l_dt_Articulo.Rows[0]["rubro_id"].ToString()); }
                    //model.RubroCod = l_dt_Articulo.Rows[0]["rubro_cod"].ToString();
                    //model.RubroNombre = l_dt_Articulo.Rows[0]["rubro_nombre"].ToString();
                    model.Descripcion2 = l_dt_Articulo.Rows[0]["articulo_desc2"].ToString();
                    model.CodigoBarras = l_dt_Articulo.Rows[0]["codigo_barras"].ToString();
                    model.Observaciones1 = l_dt_Articulo.Rows[0]["observaciones_1"].ToString();
                    model.Observaciones2 = l_dt_Articulo.Rows[0]["observaciones_2"].ToString();
                    model.Observaciones3 = l_dt_Articulo.Rows[0]["observaciones_3"].ToString();
                }

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarArticulosCarga(string sProveedorId, string sFechaCarga)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "ConsultarArticulosCarga");

                //Viene en formato YYYY-mm-DD y lo paso a formato mm/DD/YYYY
                string sMMDDYYYYCarga = sFechaCarga.Substring(5, 2) + "/" + sFechaCarga.Substring(8, 2) + "/" + sFechaCarga.Substring(0, 4);
                DataTable l_dt_Articulos = new DataTable();
                string l_s_stSql = "SELECT articulo_x_proveedor_id, articulo_cod, articulo_nombre,";
                l_s_stSql += " moneda_cod, precio_en_moneda";
                l_s_stSql += " FROM vw_articulos";
                l_s_stSql += " WHERE proveedor_id = " + sProveedorId;
                l_s_stSql += " AND fecha_carga >= CAST('" + sMMDDYYYYCarga + "' AS DATE)";
                l_s_stSql += " ORDER BY articulo_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ArticuloDAO.cs", "ConsultarArticulosCarga");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Articulos = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Articulos.Fill(l_dt_Articulos);
                }
                return l_dt_Articulos;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "ConsultarArticulosCarga");
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "ConsultarArticulos");

                DataTable l_dt_Articulos = new DataTable();
                string l_s_stSql = "SELECT articulo_x_proveedor_id, proveedor_cod, proveedor_nombre,";
                l_s_stSql += " articulo_cod, articulo_nombre, flag_activo,";
                l_s_stSql += " moneda_cod, precio_en_moneda, pendiente_recepcion";
                l_s_stSql += " FROM vw_articulos";
                l_s_stSql += " ORDER BY proveedor_cod, articulo_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ArticuloDAO.cs", "ConsultarArticulos");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Articulos = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Articulos.Fill(l_dt_Articulos);
                }
                return l_dt_Articulos;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "ConsultarArticulos");
                return null;
            }
            finally { }
        }

        public string Importar(ArticuloModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "Importar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();//                                           1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0  1
                    string l_s_stSql = "{? = CALL sp_articulosXproveedores_import (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                        param.Direction = ParameterDirection.ReturnValue;
                        //Código de proveedor
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 20);
                        if (model.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Codigo.ToUpper();
                            }
                        }
                        //Nombre de proveedor
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(model.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (model.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = model.Activo;
                        //Precio
                        param = command.Parameters.Add("precio", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PrecioEnMoneda;
                        //Id de la moneda
                        param = command.Parameters.Add("moneda_id", OdbcType.Int);
                        param.Value = model.MonedaId;
                        //Id del proveedor
                        param = command.Parameters.Add("proveedor_id", OdbcType.Int);
                        param.Value = model.ProveedorId;
                        //Id del rubro
                        param = command.Parameters.Add("rubro_id", OdbcType.Int);
                        if (model.RubroId <= 0)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.RubroId;
                        }
                        //Descripción 2
                        param = command.Parameters.Add("descripcion2", OdbcType.VarChar, 250);
                        if (model.Descripcion2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion2;
                            }
                        }
                        //Código de barras
                        param = command.Parameters.Add("codigo_barras", OdbcType.VarChar, 50);
                        if (model.CodigoBarras == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.CodigoBarras == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.CodigoBarras;
                            }
                        }
                        //Costo RMB
                        param = command.Parameters.Add("costo_rmb", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoRMB;
                        //Cotización
                        param = command.Parameters.Add("cotizacion", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.Cotizacion;
                        //Unidades por bulto
                        param = command.Parameters.Add("uni_x_bulto", OdbcType.Int);
                        param.Value = model.UniXbulto;
                        //Costo INNER
                        param = command.Parameters.Add("costo_inner", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoINNER;
                        //Costo CBM
                        param = command.Parameters.Add("costo_cbm", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoCBM;
                        //Peso neto
                        param = command.Parameters.Add("peso_neto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoNeto;
                        //Peso bruto
                        param = command.Parameters.Add("peso_bruto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoBruto;
                        //Observaciones 1
                        param = command.Parameters.Add("obs1", OdbcType.VarChar, 50);
                        if (model.Observaciones1 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones1 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones1;
                            }
                        }
                        //Observaciones 2
                        param = command.Parameters.Add("obs2", OdbcType.VarChar, 250);
                        if (model.Observaciones2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones2;
                            }
                        }
                        //Observaciones 3
                        param = command.Parameters.Add("obs3", OdbcType.VarChar, 250);
                        if (model.Observaciones3 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones3 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones3;
                            }
                        }
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 250);
                        param.Value = model.LoginCreacion;

                        OdbcDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                            l_s_Mensaje = (dr.GetString(0));
                        trx.Commit();
                    }

                }

                return l_s_Mensaje; //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Importar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(ArticuloModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();//                                           1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0  1
                    string l_s_stSql = "{? = CALL sp_articulosXproveedores_insert (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                        param.Direction = ParameterDirection.ReturnValue;
                        //Código de proveedor
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 20);
                        if (model.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Codigo.ToUpper();
                            }
                        }
                        //Nombre de proveedor
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(model.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (model.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = model.Activo;
                        //Precio
                        param = command.Parameters.Add("precio", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PrecioEnMoneda;
                        //Id de la moneda
                        param = command.Parameters.Add("moneda_id", OdbcType.Int);
                        param.Value = model.MonedaId;
                        //Id del proveedor
                        param = command.Parameters.Add("proveedor_id", OdbcType.Int);
                        param.Value = model.ProveedorId;
                        //Id del rubro
                        param = command.Parameters.Add("rubro_id", OdbcType.Int);
                        if (model.RubroId <= 0)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.RubroId;
                        }
                        //Descripción 2
                        param = command.Parameters.Add("descripcion2", OdbcType.VarChar, 250);
                        if (model.Descripcion2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion2;
                            }
                        }
                        //Código de barras
                        param = command.Parameters.Add("codigo_barras", OdbcType.VarChar, 50);
                        if (model.CodigoBarras == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.CodigoBarras == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.CodigoBarras;
                            }
                        }
                        //Costo RMB
                        param = command.Parameters.Add("costo_rmb", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoRMB;
                        //Cotización
                        param = command.Parameters.Add("cotizacion", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.Cotizacion;
                        //Unidades por bulto
                        param = command.Parameters.Add("uni_x_bulto", OdbcType.Int);
                        param.Value = model.UniXbulto;
                        //Costo INNER
                        param = command.Parameters.Add("costo_inner", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoINNER;
                        //Costo CBM
                        param = command.Parameters.Add("costo_cbm", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.CostoCBM;
                        //Peso neto
                        param = command.Parameters.Add("peso_neto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoNeto;
                        //Peso bruto
                        param = command.Parameters.Add("peso_bruto", OdbcType.Double);// con OdbcType.Decimal graba mal los decimales
                        param.Value = model.PesoBruto;
                        //Observaciones 1
                        param = command.Parameters.Add("obs1", OdbcType.VarChar, 50);
                        if (model.Observaciones1 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones1 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones1;
                            }
                        }
                        //Observaciones 2
                        param = command.Parameters.Add("obs2", OdbcType.VarChar, 250);
                        if (model.Observaciones2 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones2 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones2;
                            }
                        }
                        //Observaciones 3
                        param = command.Parameters.Add("obs3", OdbcType.VarChar, 250);
                        if (model.Observaciones3 == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Observaciones3 == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Observaciones3;
                            }
                        }
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 250);
                        param.Value = model.LoginCreacion;

                        OdbcDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                            l_s_Mensaje = (dr.GetString(0));
                        trx.Commit();
                    }

                }

                return l_s_Mensaje; //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Eliminar(int iArtXProvId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iArtXProvId.ToString() + ", " + sUsuario, "ArticuloDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_articulosXproveedores_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = iArtXProvId;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = sUsuario;

                        command.ExecuteNonQuery();
                        trx.Commit();
                    }

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

    }
}
