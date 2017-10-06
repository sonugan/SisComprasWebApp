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
    public class LocalidadBL
    {
        public DataTable ConsultarLocalidadesActivas(int localidadId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + localidadId.ToString(), "LocalidadBL.cs", "ConsultarLocalidadesActivas");

                LocalidadDAO l_dao_Localidad = new LocalidadDAO();
                DataTable l_dt_Localidades = l_dao_Localidad.ConsultarLocalidadesActivas(localidadId);

                return l_dt_Localidades;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "ConsultarLocalidadesActivas");
                return null;
            }
            finally { }
        }

        public LocalidadModel Consultar(int localidadId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + localidadId.ToString(), "LocalidadBL.cs", "Consultar");

                LocalidadModel model = new LocalidadModel();
                LocalidadDAO l_dao_Localidad = new LocalidadDAO();

                model = l_dao_Localidad.Consultar(localidadId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarLocalidades()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                LocalidadDAO l_dao_Localidad = new LocalidadDAO();
                DataTable l_dt_Localidades = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "LocalidadBL.cs", "ConsultarLocalidades");

                l_dt_Localidades = l_dao_Localidad.ConsultarLocalidades();
                return l_dt_Localidades;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "ConsultarLocalidades");
                return null;
            }
            finally { }
        }

        public string Eliminar(int localidadId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + localidadId.ToString() + ", " + sUsuario, "LocalidadBL.cs", "Eliminar");

                LocalidadDAO l_dao_Localidad = new LocalidadDAO();
                l_s_Mensaje = l_dao_Localidad.Eliminar(localidadId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(LocalidadModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                LocalidadDAO l_dao_Localidad = new LocalidadDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_LOCALIDAD")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "LocalidadBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Localidad.BuscarCodigo(l_s_Codigo, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Localidad.BuscarNombre(model.Nombre, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Localidad.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(LocalidadModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                LocalidadDAO l_dao_Localidad = new LocalidadDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_LOCALIDAD")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "LocalidadBL.cs", "Insertar");

                l_s_Mensaje = l_dao_Localidad.BuscarCodigo(l_s_Codigo, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Localidad.BuscarNombre(model.Nombre, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Localidad.Insertar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }
    }
}
