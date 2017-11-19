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
                    string l_s_stSql = "{? = CALL TEST(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
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

        #endregion Private Methods
    }
}
