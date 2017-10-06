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
    public class RubroBL
    {

        public DataTable ConsultarRubrosActivos(int rubroId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + rubroId.ToString(), "RubroBL.cs", "ConsultarRubrosActivos");

                RubroDAO l_dao_Rubro = new RubroDAO();
                DataTable l_dt_Rubros = l_dao_Rubro.ConsultarRubrosActivos(rubroId);

                return l_dt_Rubros;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "ConsultarRubrosActivos");
                return null;
            }
            finally { }
        }

        public RubroModel Consultar(int rubroId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + rubroId.ToString(), "RubroBL.cs", "Consultar");

                RubroModel model = new RubroModel();
                RubroDAO l_dao_Rubro = new RubroDAO();

                model = l_dao_Rubro.Consultar(rubroId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarRubros()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                RubroDAO l_dao_Rubro = new RubroDAO();
                DataTable l_dt_Rubros = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "RubroBL.cs", "ConsultarRubros");

                l_dt_Rubros = l_dao_Rubro.ConsultarRubros();
                return l_dt_Rubros;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "ConsultarRubros");
                return null;
            }
            finally { }
        }

        public string Eliminar(int rubroId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + rubroId.ToString() + ", " + sUsuario, "RubroBL.cs", "Eliminar");

                RubroDAO l_dao_Rubro = new RubroDAO();
                l_s_Mensaje = l_dao_Rubro.Eliminar(rubroId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(RubroModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                RubroDAO l_dao_Rubro = new RubroDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_RUBRO")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "RubroBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Rubro.BuscarCodigo(l_s_Codigo, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Rubro.BuscarNombre(model.Nombre, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Rubro.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(RubroModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                RubroDAO l_dao_Rubro = new RubroDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_RUBRO")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "RubroBL.cs", "Insertar");

                l_s_Mensaje = l_dao_Rubro.BuscarCodigo(l_s_Codigo, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Rubro.BuscarNombre(model.Nombre, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Rubro.Insertar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }
    }
}
