using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using AccesoDatos;
using System.Data;

namespace SisCompras.BL
{
    public class MonedaBL
    {
        public DataTable ConsultarMonedasActivas(int rubroId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + rubroId.ToString(), "MonedaBL.cs", "ConsultarMonedasActivas");

                MonedaDAO l_dao_Moneda = new MonedaDAO();
                DataTable l_dt_Monedas = l_dao_Moneda.ConsultarMonedasActivas(0);

                return l_dt_Monedas;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "MonedaBL.cs", "ConsultarMonedasActivas");
                return null;
            }
            finally { }
        }

        public List<MonedaModel> ConsultarMonedasActivasList(int rubroId)
        {
            MonedaDAO monedaDao = new MonedaDAO();
            return monedaDao.ConsultarMonedasActivasList(rubroId);
        }
    }
}
