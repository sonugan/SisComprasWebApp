using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using Modelos;
using Modelos.Dtos;

namespace BdEnMemoria
{
    public class OrdenCompraDAO
    {
        private OnMemoryDb<OCCabeceraModel> DbCabeceras { get; set; }
        private OnMemoryDb<OCLineaModel> DbLineas { get; set; }

        public OrdenCompraDAO()
        {
            DbCabeceras = new OnMemoryDb<OCCabeceraModel>();
            DbLineas = new OnMemoryDb<OCLineaModel>();
        }
        
        public string InsertarCabecera(OCCabeceraModel cabecera)
        {
            return DbCabeceras.Insert(cabecera).ToString();
        }

        public string InsertarLinea(OCLineaModel linea)
        {
            return DbLineas.Insert(linea).ToString();
        }

        //public OrdenCompraModel ConsultarOrdenCompra(int ordenCompraId)
        //{
        //    AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
        //    string mensaje = "";

        //    try
        //    {
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

        //        DataTable dt = new DataTable();
        //        string sql = "SELECT orden_compra_cab_id, orden_compra_nro, proveedor_id, observaciones,";
        //        sql += " numero_referencia, fecha_emision, condicion_compra_id, estado_cod, cantidad_pedida_total,";
        //        sql += " cantidad_recibida_total, importe_total, moneda_operacion_id, moneda_nacional_id, cotizacion";
        //        sql += " FROM ordenes_compra_cab";
        //        sql += " WHERE orden_compra_cab_id = " + ordenCompraId.ToString();
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

        //        using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
        //        {
        //            odbcConn.Open();
        //            OdbcDataAdapter adapter = new OdbcDataAdapter(sql, odbcConn);
        //            adapter.Fill(dt);
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            var dr = dt.Rows[0];
        //            return new OrdenCompraModel()
        //            {
        //                cabecera = new OCCabeceraModel()
        //                {
        //                    ID = Convert.ToInt32(dr["orden_compra_cab_id"]),
        //                    ProveedorId = Convert.ToInt32(dr["proveedor_id"]),
        //                    FechaEmision = dr["fecha_emision"] != null ? Convert.ToDateTime(dr["fecha_emision"]) : new DateTime(),
        //                    NroReferencia = dr["numero_referencia"] != null ? dr["numero_referencia"].ToString() : "",
        //                    CantidadTotal = dr["cantidad_pedida_total"] != null ? Convert.ToDecimal(dr["cantidad_pedida_total"]) : 0,
        //                    ImporteTotal = dr["importe_total"] != null ? Convert.ToDecimal(dr["importe_total"]) : 0,
        //                    Numero = dr["orden_compra_nro"] != null ? dr["orden_compra_nro"].ToString() : "",
        //                }
        //            };
        //        }
        //        else
        //        {
        //            return new OrdenCompraModel();
        //        }

        //    }
        //    catch (Exception miEx)
        //    {
        //        mensaje = miEx.Message.ToString();
        //        System.Diagnostics.Debug.WriteLine(mensaje);
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
        //        return null;
        //    }
        //    finally { }
        //}

        //public ListaPaginada<ArticuloOrdenCompraDto> ConsultarArticulosOrdenCompra(Paginado paginado, int cabeceraId)
        //{
        //    AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
        //    string mensaje = "";
        //    try
        //    {
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

        //        string selectArticuloOrdenCompra = string.Format(@"
        //                        SELECT o.orden_compra_linea_id, o.articulo_x_proveedor_id, o.cantidad_pedida, o.precio_unitario,
        //                        o.fecha_recepcion, o.cantidad_recibida, o.porc_descuento, o.orden_compra_cab_id, o.unidad_medida_cod,
        //                        a.articulo_cod, a.moneda_id, a.precio_en_moneda, a.proveedor_id, articulo_nombre
        //                        FROM ordenes_compra_lineas o INNER JOIN articulos_x_proveedores a ON (a.articulo_x_proveedor_id = o.articulo_x_proveedor_id)
        //                        WHERE orden_compra_cab_id = {0}", cabeceraId);

        //        //logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarArticulo");

        //        DataTable articulosOrdenCompraDt = new DataTable();
        //        using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
        //        {
        //            odbcConn.Open();
        //            OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticuloOrdenCompra, odbcConn);
        //            adapter.Fill(articulosOrdenCompraDt);
        //        }

        //        List<ArticuloOrdenCompraDto> articulos = new List<ArticuloOrdenCompraDto>();

        //        if (articulosOrdenCompraDt.Rows.Count > 0)
        //        {
        //            var dr = articulosOrdenCompraDt.Rows[0];
        //            var articulo = new ArticuloOrdenCompraDto()
        //            {
        //                CabeceraId = cabeceraId,
        //                ID = Convert.ToInt32(dr["orden_compra_linea_id"]),
        //                Cantidad = Convert.ToInt32(dr["cantidad_pedida"]),
        //                ArticuloId = Convert.ToInt32(dr["articulo_x_proveedor_id"]),
        //                CodigoArticulo = dr["articulo_cod"].ToString(),
        //                ProveedorId = Convert.ToInt32(dr["proveedor_id"]),
        //                NombreArticulo = dr["articulo_nombre"].ToString()
        //            };
        //            articulos.Add(articulo);
        //        }

        //        Paginador<ArticuloOrdenCompraDto> paginador = new Paginador<ArticuloOrdenCompraDto>();
        //        return paginador.Paginar(articulos, paginado);

        //    }
        //    catch (Exception miEx)
        //    {
        //        mensaje = miEx.Message.ToString();
        //        System.Diagnostics.Debug.WriteLine(mensaje);
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
        //        return null;
        //    }
        //    finally { }
        //}

        //public ArticuloOrdenCompraDto ConsultarArticulo(int articuloId, int ordenCompraId)
        //{
        //    AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
        //    string mensaje = "";
        //    try
        //    {
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");

        //        string selectArticulo = string.Format(@"
        //                        SELECT 
        //                        a.articulo_x_proveedor_id, a.articulo_cod, a.moneda_id, a.precio_en_moneda, a.proveedor_id
        //                        FROM articulos_x_proveedores a 
        //                        WHERE articulo_x_proveedor_id = {0}", articuloId);

        //        DataTable articuloDt = new DataTable();
        //        using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
        //        {
        //            odbcConn.Open();
        //            OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticulo, odbcConn);
        //            adapter.Fill(articuloDt);
        //        }

        //        var articulo = new ArticuloOrdenCompraDto()
        //        {
        //            CabeceraId = ordenCompraId
        //        };
        //        if (articuloDt.Rows.Count > 0)
        //        {
        //            var dr = articuloDt.Rows[0];
        //            articulo.ArticuloId = Convert.ToInt32(dr["articulo_x_proveedor_id"]);
        //            articulo.CodigoArticulo = dr["articulo_cod"].ToString();
        //            articulo.ProveedorId = Convert.ToInt32(dr["proveedor_id"]);
        //        }

        //        string selectArticuloOrdenCompra = string.Format(@"
        //                        SELECT o.orden_compra_linea_id, o.articulo_x_proveedor_id, o.cantidad_pedida, o.precio_unitario,
        //                        o.fecha_recepcion, o.cantidad_recibida, o.porc_descuento, o.orden_compra_cab_id, o.unidad_medida_cod
        //                        FROM ordenes_compra_lineas o
        //                        WHERE orden_compra_cab_id = {0} 
        //                          AND articulo_x_proveedor_id = {1}", ordenCompraId, articuloId);

        //        //logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, sql, "OrdenCompraDAO.cs", "ConsultarArticulo");

        //        DataTable articulosOrdenCompraDt = new DataTable();
        //        using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
        //        {
        //            odbcConn.Open();
        //            OdbcDataAdapter adapter = new OdbcDataAdapter(selectArticuloOrdenCompra, odbcConn);
        //            adapter.Fill(articulosOrdenCompraDt);
        //        }
        //        if (articulosOrdenCompraDt.Rows.Count > 0)
        //        {
        //            var dr = articulosOrdenCompraDt.Rows[0];
        //            if (dr["orden_compra_linea_id"] != null)
        //            {
        //                articulo.ID = Convert.ToInt32(dr["orden_compra_linea_id"]);
        //                articulo.Cantidad = Convert.ToInt32(dr["cantidad_pedida"]);
        //                articulo.CabeceraId = Convert.ToInt32(dr["orden_compra_cab_id"]);
        //            }

        //            return articulo;
        //        }

        //        return articulo;

        //    }
        //    catch (Exception miEx)
        //    {
        //        mensaje = miEx.Message.ToString();
        //        System.Diagnostics.Debug.WriteLine(mensaje);
        //        logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraDAO.cs", "ConsultarOrdenesCompra");
        //        return null;
        //    }
        //    finally { }
        //}

        //public string AgregarArticulo(ArticuloOrdenCompraDto articuloOrdenCompra)
        //{
        //    string resultado = "";
        //    try
        //    {
        //        InsertarLogEntrante("AgregarArticulo");

        //        using (OdbcConnection connection = new OdbcConnection(connectionString))
        //        {
        //            connection.Open();
        //            OdbcTransaction trx = connection.BeginTransaction();
        //            string l_s_stSql = "{? = CALL SP_ORDENES_COMPRA_LINEAS_UPDATE(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        //            using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
        //            {
        //                command.Transaction = trx;
        //                command.CommandType = CommandType.StoredProcedure;

        //                AgregarParametroOutput(command, "PO_I_ORDEN_COMPRA_LINEA_ID", OdbcType.Int);
        //                AgregarParametroInput(command, "ORDEN_COMPRA_LINEA_ID", OdbcType.Int, articuloOrdenCompra.ID);
        //                AgregarParametroInput(command, "ARTICULO_X_PROVEEDOR_ID", OdbcType.Int, articuloOrdenCompra.ArticuloId);
        //                AgregarParametroInput(command, "CANTIDAD_PEDIDA", OdbcType.Numeric, articuloOrdenCompra.Cantidad);
        //                AgregarParametroInput(command, "PRECIO_UNITARIO", OdbcType.Numeric, articuloOrdenCompra.Precio);
        //                AgregarParametroInput(command, "FECHA_RECEPCION", OdbcType.Date, articuloOrdenCompra.FechaRecepcion);
        //                AgregarParametroInput(command, "CANTIDAD_RECIBIDA", OdbcType.Numeric, articuloOrdenCompra.Recibido);
        //                AgregarParametroInput(command, "PORC_DESCUENTO", OdbcType.Numeric, articuloOrdenCompra.PorcDescuento);
        //                AgregarParametroInput(command, "ORDEN_COMPRA_CAB_ID", OdbcType.Int, articuloOrdenCompra.CabeceraId);
        //                AgregarParametroInput(command, "UNIDAD_MEDIDA_COD", OdbcType.VarChar, 50, null);//TODO: ver de donde sacar este campo
        //                AgregarParametroInput(command, "LOGIN_ULT_MODIF", OdbcType.VarChar, 50, articuloOrdenCompra.LoginUltModif);

        //                OdbcDataReader dr = command.ExecuteReader();
        //                while (dr.Read())
        //                    resultado = (dr.GetString(0));
        //                trx.Commit();
        //            }

        //        }

        //        return resultado ?? articuloOrdenCompra.ID.ToString(); //devuelve el Id
        //    }
        //    catch (Exception miEx)
        //    {
        //        resultado = miEx.Message.ToString();
        //        System.Diagnostics.Debug.WriteLine(resultado);
        //        loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, resultado, "ArticuloDAO.cs", "AgregarArticulo");
        //        return resultado;
        //    }
        //    finally { }
        //}

        #region Private Methods
        

        #endregion Private Methods
    }
}
