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
    public class OrdenCompraDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

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
    }
}
