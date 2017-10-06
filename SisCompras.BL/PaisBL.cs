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
    public class PaisBL
    {
        public DataTable ConsultarPaisesActivos(int paisId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + paisId.ToString(), "PaisBL.cs", "ConsultarPaisesActivos");

                PaisDAO l_dao_Pais = new PaisDAO();
                DataTable l_dt_Paises = l_dao_Pais.ConsultarPaisesActivos(paisId);

                return l_dt_Paises;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "ConsultarPaisesActivos");
                return null;
            }
            finally { }
        }

        public PaisModel Consultar(int paisId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + paisId.ToString(), "PaisBL.cs", "Consultar");

                PaisModel model = new PaisModel();
                PaisDAO l_dao_Pais = new PaisDAO();

                model = l_dao_Pais.Consultar(paisId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarPaises()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                PaisDAO l_dao_Pais = new PaisDAO();
                DataTable l_dt_Paises = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "PaisBL.cs", "ConsultarPaises");

                l_dt_Paises = l_dao_Pais.ConsultarPaises();
                return l_dt_Paises;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "ConsultarPaises");
                return null;
            }
            finally { }
        }

        public string Eliminar(int paisId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + paisId.ToString() + ", " + sUsuario, "PaisBL.cs", "Eliminar");

                PaisDAO l_dao_Pais = new PaisDAO();
                l_s_Mensaje = l_dao_Pais.Eliminar(paisId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(PaisModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                PaisDAO l_dao_Pais = new PaisDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PAIS")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "PaisBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Pais.BuscarCodigo(l_s_Codigo, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.BuscarNombre(model.Nombre, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.BuscarCodigoAFIP(model.CodigoAFIP, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(PaisModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                PaisDAO l_dao_Pais = new PaisDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PAIS")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "PaisBL.cs", "Insertar");

                l_s_Mensaje = l_dao_Pais.BuscarCodigo(l_s_Codigo, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.BuscarNombre(model.Nombre, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.BuscarCodigoAFIP(model.CodigoAFIP, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Pais.Insertar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

    }
}
