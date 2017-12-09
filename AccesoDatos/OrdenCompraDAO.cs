using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using Modelos;
using AplicacionLog;
using Modelos.Dtos;

namespace AccesoDatos
{
    public class OrdenCompraDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        private Logueo loger;
        private GeneralesDAO generalesDao;

        public OrdenCompraDAO()
        {
            loger = new Logueo();
            generalesDao = new GeneralesDAO();
        }

        public DataTable ConsultarOrdenesCompra()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                DataTable l_dt_OCompras = new DataTable();
                string l_s_stSql = "SELECT orden_compra_cab_id, proveedor_cod, proveedor_nombre,";
                l_s_stSql += " fecha_emision, numero_referencia, cantidad_pedida_total,";
                l_s_stSql += " moneda_operacion_cod, importe_total, orden_compra_nro";
                l_s_stSql += " FROM vw_ordenes_compra_cab";
                l_s_stSql += " ORDER BY fecha_emision DESC, proveedor_cod ASC, orden_compra_nro DESC";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_OCompras = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_OCompras.Fill(l_dt_OCompras);
                }
                return l_dt_OCompras;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        public string Insertar(OrdenCompraModel ordenDeCompra)
        {
            string resultado = "";
            try
            {
                InsertarLogEntrante("Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    //string l_s_stSql = "{? = CALL SP_ORDENES_COMPRA_CAB_INSERT(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();
                    string l_s_stSql = "{? = CALL SP_ORDENES_COMPRA_CAB_INSERT(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        AgregarParametroOutput(command, "PO_I_ORDEN_COMPRA_ID", OdbcType.Int);
                        AgregarParametroInput(command, "NUMERO", OdbcType.VarChar, 30, ordenDeCompra.cabecera.Numero);
                        AgregarParametroInput(command, "PROVEEDOR_ID", OdbcType.Int, ordenDeCompra.cabecera.ProveedorId);
                        AgregarParametroInput(command, "OBSERVACIONES", OdbcType.VarChar, 250, ordenDeCompra.cabecera.Observaciones);
                        AgregarParametroInput(command, "NUMERO_REFERENCIA", OdbcType.VarChar, 30, ordenDeCompra.cabecera.NroReferencia);
                        AgregarParametroInput(command, "FECHA_EMISION", OdbcType.Date, ordenDeCompra.cabecera.FechaEmision);
                        AgregarParametroInput(command, "CONDICION_COMPRA_ID", OdbcType.Int, ordenDeCompra.cabecera.CondicionCompraId);
                        AgregarParametroInput(command, "ESTADO_COD", OdbcType.VarChar, 50, OrdenCompraModel.Estados.INICIADA.ToString());
                        AgregarParametroInput(command, "NUMERO_VERSION", OdbcType.Int, ordenDeCompra.cabecera.Version);
                        AgregarParametroInput(command, "LOGIN_CREACION", OdbcType.VarChar, 50, ordenDeCompra.cabecera.LoginCreacion);
                        AgregarParametroInput(command, "LOGIN_ULT_MODIF", OdbcType.VarChar, 50, ordenDeCompra.cabecera.LoginUltModif);
                        AgregarParametroInput(command, "MONEDA_OPERACION_ID", OdbcType.Int, ordenDeCompra.cabecera.MonedaOperacionId);
                        AgregarParametroInput(command, "COTIZACION", OdbcType.Numeric, ordenDeCompra.cabecera.Cotizacion);

                        OdbcDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                            resultado = (dr.GetString(0));
                        trx.Commit();
                    }

                }

                return resultado; //devuelve el Id
            }
            catch (Exception miEx)
            {
                resultado = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(resultado);
                loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, resultado, "ArticuloDAO.cs", "Insertar");
                return resultado;
            }
            finally { }
        }

        public OrdenCompraModel ConsultarOrdenCompra(int ordenCompraId)
        {
            AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
            string mensaje = "";

            try
            {
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                DataTable dt = new DataTable();
                string sql = "SELECT orden_compra_cab_id, orden_compra_nro, proveedor_id, observaciones,";
                sql += " numero_referencia, fecha_emision, condicion_compra_id, estado_cod, cantidad_pedida_total,";
                sql += " cantidad_recibida_total, importe_total, moneda_operacion_id, moneda_nacional_id, cotizacion";
                sql += " FROM ordenes_compra_cab";
                sql += " WHERE orden_compra_cab_id = " + ordenCompraId.ToString();
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(sql, odbcConn);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    var dr = dt.Rows[0];
                    return new OrdenCompraModel()
                    {
                        cabecera = new OCCabeceraModel()
                        {
                            ID = Convert.ToInt32(dr["orden_compra_cab_id"]),
                            ProveedorId = Convert.ToInt32(dr["proveedor_id"]),
                            FechaEmision = dr["fecha_emision"] != null ? Convert.ToDateTime(dr["fecha_emision"]) : new DateTime(),
                            NroReferencia = dr["numero_referencia"] != null ? dr["numero_referencia"].ToString() : "",
                            CantidadTotal = dr["cantidad_pedida_total"] != null ? Convert.ToDecimal(dr["cantidad_pedida_total"]) : 0,
                            ImporteTotal = dr["importe_total"] != null ? Convert.ToDecimal(dr["importe_total"]) : 0,
                            Numero = dr["orden_compra_nro"] != null ? dr["orden_compra_nro"].ToString() : "",
                        }
                    };
                }
                else
                {
                    return new OrdenCompraModel();
                }

            }
            catch (Exception miEx)
            {
                mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(mensaje);
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        public ListaPaginada<ArticuloOrdenCompraDto> ConsultarArticulosOrdenCompra(Paginado paginado, int cabeceraId)
        {
            AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
            string mensaje = "";
            try
            {
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                string selectArticuloOrdenCompra = string.Format(@"
                                SELECT o.orden_compra_linea_id, o.articulo_x_proveedor_id, o.cantidad_pedida, o.precio_unitario,
                                o.fecha_recepcion, o.cantidad_recibida, o.porc_descuento, o.orden_compra_cab_id, o.unidad_medida_cod,
                                a.articulo_cod, a.moneda_id, a.precio_en_moneda, a.proveedor_id, articulo_nombre, a.url_imagen
                                FROM ordenes_compra_lineas o INNER JOIN articulos_x_proveedores a ON (a.articulo_x_proveedor_id = o.articulo_x_proveedor_id)
                                WHERE orden_compra_cab_id = {0}", cabeceraId);

                //logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarArticulo");

                DataTable articulosOrdenCompraDt = new DataTable();
                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticuloOrdenCompra, odbcConn);
                    adapter.Fill(articulosOrdenCompraDt);
                }

                List<ArticuloOrdenCompraDto> articulos = new List<ArticuloOrdenCompraDto>();

                if (articulosOrdenCompraDt.Rows.Count > 0)
                {
                    var dr = articulosOrdenCompraDt.Rows[0];
                    var articulo = new ArticuloOrdenCompraDto()
                    {
                        CabeceraId = cabeceraId,
                        ID = Convert.ToInt32(dr["orden_compra_linea_id"]),
                        Cantidad = Convert.ToInt32(dr["cantidad_pedida"]),
                        ArticuloId = Convert.ToInt32(dr["articulo_x_proveedor_id"]),
                        CodigoArticulo = dr["articulo_cod"].ToString(),
                        ProveedorId = Convert.ToInt32(dr["proveedor_id"]),
                        NombreArticulo = dr["articulo_nombre"].ToString(),
                        FotoArticulo = FotoGenerator(dr["url_imagen"].ToString())
                    };
                    articulos.Add(articulo);
                }

                Paginador<ArticuloOrdenCompraDto> paginador = new Paginador<ArticuloOrdenCompraDto>();
                return paginador.Paginar(articulos, paginado);

            }
            catch (Exception miEx)
            {
                mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(mensaje);
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        private Foto FotoGenerator(string path)
        {
            var fullPath = path;
            if (!System.IO.Path.IsPathRooted(path))
            {
                fullPath = GetFullPath(path);
            }
            if (!string.IsNullOrEmpty(fullPath) && System.IO.File.Exists(fullPath))
            {
                return new Foto(fullPath);
            }
            return new Foto(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, "Content\\Imagenes\\noImage.png"));
        }

        private string GetFullPath(string path)
        {
            return string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, path);
        }

        public ArticuloOrdenCompraDto ConsultarArticulo(int articuloId, int ordenCompraId)
        {
            AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
            string mensaje = "";
            try
            {
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

                string selectArticulo = string.Format(@"
                                SELECT 
                                a.articulo_x_proveedor_id, a.articulo_cod, a.moneda_id, a.precio_en_moneda, a.proveedor_id
                                FROM articulos_x_proveedores a 
                                WHERE articulo_x_proveedor_id = {0}", articuloId);

                DataTable articuloDt = new DataTable();
                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticulo, odbcConn);
                    adapter.Fill(articuloDt);
                }

                var articulo = new ArticuloOrdenCompraDto()
                {
                    CabeceraId = ordenCompraId
                };
                if (articuloDt.Rows.Count > 0)
                {
                    var dr = articuloDt.Rows[0];
                    articulo.ArticuloId = Convert.ToInt32(dr["articulo_x_proveedor_id"]);
                    articulo.CodigoArticulo = dr["articulo_cod"].ToString();
                    articulo.ProveedorId = Convert.ToInt32(dr["proveedor_id"]);
                }

                string selectArticuloOrdenCompra = string.Format(@"
                                SELECT o.orden_compra_linea_id, o.articulo_x_proveedor_id, o.cantidad_pedida, o.precio_unitario,
                                o.fecha_recepcion, o.cantidad_recibida, o.porc_descuento, o.orden_compra_cab_id, o.unidad_medida_cod
                                FROM ordenes_compra_lineas o
                                WHERE orden_compra_cab_id = {0} 
                                  AND articulo_x_proveedor_id = {1}", ordenCompraId, articuloId);

                //logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarArticulo");

                DataTable articulosOrdenCompraDt = new DataTable();
                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticuloOrdenCompra, odbcConn);
                    adapter.Fill(articulosOrdenCompraDt);
                }
                if (articulosOrdenCompraDt.Rows.Count > 0)
                {
                    var dr = articulosOrdenCompraDt.Rows[0];
                    if (dr["orden_compra_linea_id"] != null)
                    {
                        articulo.ID = Convert.ToInt32(dr["orden_compra_linea_id"]);
                        articulo.Cantidad = Convert.ToInt32(dr["cantidad_pedida"]);
                        articulo.CabeceraId = Convert.ToInt32(dr["orden_compra_cab_id"]);
                    }

                    return articulo;
                }

                return articulo;

            }
            catch (Exception miEx)
            {
                mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(mensaje);
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        public string AgregarArticulo(ArticuloOrdenCompraDto articuloOrdenCompra)
        {
            string resultado = "";
            try
            {
                InsertarLogEntrante("AgregarArticulo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    OdbcTransaction trx = connection.BeginTransaction();
                    string l_s_stSql = "{? = CALL SP_ORDENES_COMPRA_LINEAS_UPDATE(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        AgregarParametroOutput(command, "PO_I_ORDEN_COMPRA_LINEA_ID", OdbcType.Int);
                        AgregarParametroInput(command, "ORDEN_COMPRA_LINEA_ID", OdbcType.Int, articuloOrdenCompra.ID);
                        AgregarParametroInput(command, "ARTICULO_X_PROVEEDOR_ID", OdbcType.Int, articuloOrdenCompra.ArticuloId);
                        AgregarParametroInput(command, "CANTIDAD_PEDIDA", OdbcType.Numeric, articuloOrdenCompra.Cantidad);
                        AgregarParametroInput(command, "PRECIO_UNITARIO", OdbcType.Numeric, articuloOrdenCompra.Precio);
                        AgregarParametroInput(command, "FECHA_RECEPCION", OdbcType.Date, articuloOrdenCompra.FechaRecepcion);
                        AgregarParametroInput(command, "CANTIDAD_RECIBIDA", OdbcType.Numeric, articuloOrdenCompra.Recibido);
                        AgregarParametroInput(command, "PORC_DESCUENTO", OdbcType.Numeric, articuloOrdenCompra.PorcDescuento);
                        AgregarParametroInput(command, "ORDEN_COMPRA_CAB_ID", OdbcType.Int, articuloOrdenCompra.CabeceraId);
                        AgregarParametroInput(command, "UNIDAD_MEDIDA_COD", OdbcType.VarChar, 50, null);//TODO: ver de donde sacar este campo
                        AgregarParametroInput(command, "LOGIN_ULT_MODIF", OdbcType.VarChar, 50, articuloOrdenCompra.LoginUltModif);

                        OdbcDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                            resultado = (dr.GetString(0));
                        trx.Commit();
                    }

                }

                return resultado ?? articuloOrdenCompra.ID.ToString(); //devuelve el Id
            }
            catch (Exception miEx)
            {
                resultado = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(resultado);
                loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, resultado, "ArticuloDAO.cs", "AgregarArticulo");
                return resultado;
            }
            finally { }
        }

        #region Private Methods
        private void InsertarLogEntrante(string metodo)
        {
            loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", metodo);
        }

        private OdbcParameter AgregarParametroInput(OdbcCommand command, string nombre, OdbcType tipo, int capacidad, string valor)
        {
            OdbcParameter parametro = command.Parameters.Add(nombre, tipo, capacidad);
            if (string.IsNullOrEmpty(valor))
            {
                parametro.Value = DBNull.Value;
            }
            else
            {
                parametro.Value = valor;
            }
            return parametro;
        }

        private OdbcParameter AgregarParametroInput(OdbcCommand command, string nombre, OdbcType tipo, object valor)
        {
            OdbcParameter parametro = command.Parameters.Add(nombre, tipo);
            parametro.Value = valor;
            return parametro;
        }

        private OdbcParameter AgregarParametroOutput(OdbcCommand command, string nombre, OdbcType tipo)
        {
            OdbcParameter parametro = command.Parameters.Add(nombre, tipo);
            parametro.Direction = ParameterDirection.ReturnValue;
            return parametro;
        }

        //private class StoreProcedure
        //{
        //    private string Nombre { get; set; }
        //    private OdbcCommand Command { get; set; } 
            
        //    public StoreProcedure(string nombre, OdbcCommand command)
        //    {
        //        Nombre = nombre;
        //        Command = command;
        //    }
            
        //    public OdbcDataReader ExcecuteReader()
        //    {

        //    }

        //    private void AgregarParameroInputStringEjecucion()
        //    {

        //    }

        //    public OdbcParameter AgregarParametroInput(string nombre, OdbcType tipo, int capacidad, string valor)
        //    {
        //        OdbcParameter parametro = Command.Parameters.Add(nombre, tipo, capacidad);
        //        if (string.IsNullOrEmpty(valor))
        //        {
        //            parametro.Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            parametro.Value = valor;
        //        }
        //        return parametro;
        //    }

        //    public OdbcParameter AgregarParametroInput(string nombre, OdbcType tipo, object valor)
        //    {
        //        OdbcParameter parametro = Command.Parameters.Add(nombre, tipo);
        //        parametro.Value = valor;
        //        return parametro;
        //    }

        //    public OdbcParameter AgregarParametroOutput(string nombre, OdbcType tipo)
        //    {
        //        OdbcParameter parametro = Command.Parameters.Add(nombre, tipo);
        //        parametro.Direction = ParameterDirection.ReturnValue;
        //        return parametro;
        //    }
        //}

        #endregion Private Methods
    }
}
