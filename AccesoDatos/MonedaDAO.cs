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
    public class MonedaDAO
    {

        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public int ObtenerId(string sMonedaCod)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int iMonedaId = 0;
            
            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "MonedaDAO.cs", "ObtenerId");

                string l_s_stSql = "";
                OdbcDataReader l_dr_Moneda;

                l_s_stSql = "SELECT moneda_id";
                l_s_stSql += " FROM monedas";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                l_s_stSql += " AND moneda_cod = '" + sMonedaCod + "'";

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "MonedaDAO.cs", "ObtenerId");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Moneda = cmd.ExecuteReader();
                    if (l_dr_Moneda.HasRows)
                    {
                        iMonedaId = Convert.ToInt32(l_dr_Moneda.GetValue(0));
                    }
                    cmd.Dispose();

                }

                return iMonedaId;
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "MonedaDAO.cs", "ObtenerId");
                return -1;
            }
            finally { }
        }

        public DataTable ConsultarMonedasActivas(int iMonedaId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iMonedaId.ToString(), "MonedaDAO.cs", "ConsultarMonedasActivas");

                DataTable l_dt_Monedas = new DataTable();
                string l_s_stSql = "SELECT moneda_id, moneda_cod, moneda_nombre, flag_nacional, flag_default";
                l_s_stSql += " FROM monedas";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if(iMonedaId > 0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT moneda_id, moneda_cod, moneda_nombre, flag_nacional, flag_default";
                    l_s_stSql += " FROM monedas";
                    l_s_stSql += " WHERE moneda_id = " + iMonedaId.ToString();
                }
                l_s_stSql += " ORDER BY 2";//moneda_cod
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "MonedaDAO.cs", "ConsultarMonedasActivas");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Monedas = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Monedas.Fill(l_dt_Monedas);
                }
                return l_dt_Monedas;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "MonedaDAO.cs", "ConsultarMonedasActivas");
                return null;
            }
            finally { }
        }

        public string ObtenerMonedaNacional(out int iMonedaId, out string sMonedaCod)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            iMonedaId = 0;
            sMonedaCod = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "MonedaDAO.cs", "ObtenerMonedaNacional");

                string l_s_stSql = "";
                OdbcDataReader l_dr_Moneda;

                l_s_stSql = "SELECT moneda_id, moneda_cod";
                l_s_stSql += " FROM monedas";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                l_s_stSql += " AND flag_nacional = 'Si'";

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "MonedaDAO.cs", "ObtenerMonedaNacional");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Moneda = cmd.ExecuteReader();
                    if (l_dr_Moneda.HasRows)
                    {
                        iMonedaId = Convert.ToInt32(l_dr_Moneda.GetValue(0));
                        sMonedaCod = l_dr_Moneda.GetString(1);
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "MonedaDAO.cs", "ObtenerMonedaNacional");
                return l_s_Mensaje;
            }
            finally { }
        }

    }
}
