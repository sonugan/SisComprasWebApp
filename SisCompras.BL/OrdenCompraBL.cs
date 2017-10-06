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
    public class OrdenCompraBL
    {
        public DataTable ConsultarOrdenesCompra()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                OrdenCompraDAO l_dao_OCompra = new OrdenCompraDAO();
                DataTable l_dt_OCompra = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraBL.cs", "ConsultarOrdenesCompra");

                l_dt_OCompra = l_dao_OCompra.ConsultarOrdenesCompra();
                return l_dt_OCompra;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraBL.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

    }
}
