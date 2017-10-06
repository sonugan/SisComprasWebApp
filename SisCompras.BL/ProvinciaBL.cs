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
    public class ProvinciaBL
    {
        public DataTable ConsultarProvinciasActivas(int provinciaId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + provinciaId.ToString(), "ProvinciaBL.cs", "ConsultarProvinciasActivas");

                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();
                DataTable l_dt_Provincias = l_dao_Provincia.ConsultarProvinciasActivas(provinciaId);

                return l_dt_Provincias;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "ConsultarProvinciasActivas");
                return null;
            }
            finally { }
        }

        public ProvinciaModel Consultar(int provinciaId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + provinciaId.ToString(), "ProvinciaBL.cs", "Consultar");

                ProvinciaModel model = new ProvinciaModel();
                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();

                model = l_dao_Provincia.Consultar(provinciaId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarProvincias()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();
                DataTable l_dt_Provincias = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProvinciaBL.cs", "ConsultarProvincias");

                l_dt_Provincias = l_dao_Provincia.ConsultarProvincias();
                return l_dt_Provincias;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "ConsultarProvincias");
                return null;
            }
            finally { }
        }

        public string Eliminar(int provinciaId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + provinciaId.ToString() + ", " + sUsuario, "ProvinciaBL.cs", "Eliminar");

                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();
                l_s_Mensaje = l_dao_Provincia.Eliminar(provinciaId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ProvinciaModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();
                string l_s_Codigo = model.Codigo.ToUpper();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "ProvinciaBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Provincia.BuscarCodigo(l_s_Codigo, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Provincia.BuscarNombre(model.Nombre, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Provincia.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(ProvinciaModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ProvinciaDAO l_dao_Provincia = new ProvinciaDAO();
                string l_s_Codigo = model.Codigo.ToUpper();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "ProvinciaBL.cs", "Insertar");

                l_s_Mensaje = l_dao_Provincia.BuscarCodigo(l_s_Codigo, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Provincia.BuscarNombre(model.Nombre, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Provincia.Insertar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }
    }
}
