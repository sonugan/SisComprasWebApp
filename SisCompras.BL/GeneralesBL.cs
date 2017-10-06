using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using AccesoDatos;

namespace SisCompras.BL
{
    public class GeneralesBL
    {
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + sConjuntoCod, "GeneralesBL.cs", "ConsultarConjuntoValores");

                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                DataTable l_dt_Valores = l_dao_Generales.ConsultarConjuntoValores(sConjuntoCod, sValorCod, sOrderBy, sWhere);

                return l_dt_Valores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "GeneralesBL.cs", "ConsultarConjuntoValores");
                return null;
            }
            finally { }
        }

        public int RandomNumber()
        {
            Random RandomClass = new Random();
            int randomNumber = 0;

            randomNumber = RandomClass.Next(1, 13);
            return randomNumber ;
        }
    }
}
