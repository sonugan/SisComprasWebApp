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
        private GeneralesDAO generalesDao = new GeneralesDAO();

        public OrdenCompraDAO()
        {
            loger = new AplicacionLog.Logueo();
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
            string l_s_Mensaje = "";
            try
            {
                loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloDAO.cs", "Insertar");

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
                        param.Value = generalesDao.InitCap(model.Nombre);
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
                loger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }
    }
}
