using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace AplicacionLog
{
    public class Logueo
    {

        public const short LOGL_DEBUG = 3;
        public const short LOGL_INFO = 2;
        public const short LOGL_WARN = 1;
        public const short LOGL_ERROR = 0;

        public void RegistraEnArchivoLog(short sNivelLog, string sInput = "", string sArchivoFuente = "", string sRutina = "")
        {
            string l_s_Archivo = null;
            string l_s_Mensaje = "";
            DateTime rightNow = DateTime.Now;
            string strCurrentDateTimeString = null;
            string strCurrentDateString = null;
            short NivelLog = Convert.ToInt16(ConfigurationManager.AppSettings["NivelLogueo"]);

            try
            {

                if (sNivelLog > NivelLog) { return; }

                l_s_Archivo = ConfigurationManager.AppSettings["ArchivoLog"];
                strCurrentDateString = rightNow.ToString("yyyyMMdd");
                strCurrentDateTimeString = rightNow.ToString("dd/MM/yyyy HH:mm:ss");
                l_s_Archivo += strCurrentDateString + ".log";
                //http://msdn.microsoft.com/es-es/library/36b93480(v=vs.80).aspx

                l_s_Mensaje = strCurrentDateTimeString + " [" + sNivelLog.ToString() + "] " + sArchivoFuente + " - " + sRutina + " - " + sInput;

                if (File.Exists(l_s_Archivo))
                {
                    using (StreamWriter sw = File.AppendText(l_s_Archivo))
                    {
                        sw.WriteLine(l_s_Mensaje);
                        sw.Close();
                    }

                }
                else
                {
                    using (StreamWriter sw = File.CreateText(l_s_Archivo))
                    {
                        sw.WriteLine(l_s_Mensaje);
                        sw.Close();
                    }

                }

            }
            catch
            {
                return;
            }

        }

    }
}
