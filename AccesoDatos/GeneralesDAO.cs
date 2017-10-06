using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace AccesoDatos
{
    public class GeneralesDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public string InitCap(string sTexto)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sTexto.ToLower());
        }

        public DataTable ConsultarConjuntoValores(string sConjuntoCod, string sValorCod, string sOrderBy, string sWhere)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + sConjuntoCod, "GeneralesDAO.cs", "ConsultarConjuntoValores");

                DataTable l_dt_Valores = new DataTable();
                string l_s_OrderBy = "";
                string l_s_Where = "";
                string l_s_stSql = "SELECT conjunto_valor_id, valor_codigo, valor_desc, comportamiento";
                l_s_stSql += ",multiple_uso_01, multiple_uso_02, multiple_uso_03, flag_default";
                l_s_stSql += ",multiple_uso_04, multiple_uso_05";
                l_s_stSql += " FROM sp_conjuntos_valores_buscar_conjunto ( '" + sConjuntoCod + "' )";

                if (sWhere == "") { l_s_Where = "1=1"; }
                else { l_s_Where = sWhere; }

                if (sOrderBy == "") { l_s_OrderBy = "valor_desc"; }
                else { l_s_OrderBy = sOrderBy; }

                l_s_stSql += " WHERE " + l_s_Where;
                l_s_stSql += " ORDER BY " + l_s_OrderBy;

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "GeneralesDAO.cs", "ConsultarConjuntoValores");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Valores = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Valores.Fill(l_dt_Valores);
                }
                return l_dt_Valores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "GeneralesDAO.cs", "ConsultarConjuntoValores");
                return null;
            }
            finally { }
        }

        public string ObtenerOpcionSistema(string sOpcionCod)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            string l_s_stSql = "";
            string sResultado = "";

            l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "GeneralesDAO.cs", "ObtenerOpcionSistema");

            l_s_stSql = "SELECT po_v_opcionValor";
            l_s_stSql += " FROM sp_sistema_opciones_buscar_valor('" + sOpcionCod + "',NULL,NULL)";
            l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "GeneralesDAO.cs", "ObtenerOpcionSistema");

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                sResultado = cmd.ExecuteScalar().ToString();
                cmd.Dispose();

            }

            l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "sResultado=" + sResultado, "GeneralesDAO.cs", "ObtenerOpcionSistema");
            return sResultado;
        }
    }
}
