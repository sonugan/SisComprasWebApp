using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using AccesoDatos;
using System.Data;

namespace SisCompras.BL
{
    public class ProveedorBL
    {

        public DataTable ConsultarProveedoresActivos(int proveedorId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + proveedorId.ToString(), "ProveedorBL.cs", "ConsultarProveedoresActivos");

                ProveedorDAO l_dao_Proveedor = new ProveedorDAO();
                DataTable l_dt_Proveedores = l_dao_Proveedor.ConsultarProveedoresActivos(proveedorId);

                return l_dt_Proveedores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "ConsultarProveedoresActivos");
                return null;
            }
            finally { }
        }

        public List<ProveedorModel> ConsultarProveedoresActivos()
        {

            AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
            string mensaje = "";

            try
            {
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorBL.cs", "ConsultarProveedoresActivos");

                ProveedorDAO proveedorDao = new ProveedorDAO();
                
                return proveedorDao.ConsultarProveedoresActivos();

            }
            catch (Exception miEx)
            {
                mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(mensaje);
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "ProveedorBL.cs", "ConsultarProveedoresActivos");
                return null;
            }
            finally { }
        }

        public ProveedorModel Consultar(int proveedorId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + proveedorId.ToString(), "ProveedorBL.cs", "Consultar");

                ProveedorModel model = new ProveedorModel();
                ProveedorDAO l_dao_Proveedor = new ProveedorDAO();

                model = l_dao_Proveedor.Consultar(proveedorId);

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public DataTable ConsultarProveedores()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                ProveedorDAO l_dao_Proveedor = new ProveedorDAO();
                DataTable l_dt_Proveedores = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorBL.cs", "ConsultarProveedores");

                l_dt_Proveedores = l_dao_Proveedor.ConsultarProveedores();
                return l_dt_Proveedores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "ConsultarProveedores");
                return null;
            }
            finally { }
        }

        public string Eliminar(int proveedorId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + proveedorId.ToString() + ", " + sUsuario, "ProveedorBL.cs", "Eliminar");

                ProveedorBL l_dao_Proveedor = new ProveedorBL();
                l_s_Mensaje = l_dao_Proveedor.Eliminar(proveedorId, sUsuario);

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ProveedorModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ProveedorDAO l_dao_Proveedor = new ProveedorDAO();
                string l_s_Codigo = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PROVEEDOR")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "ProveedorBL.cs", "Actualizar");

                l_s_Mensaje = l_dao_Proveedor.BuscarCodigo(l_s_Codigo, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Proveedor.BuscarNombre(model.Nombre, model.ID);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Proveedor.Actualizar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(ProveedorModel model)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                GeneralesDAO l_dao_Generales = new GeneralesDAO();
                ProveedorDAO l_dao_Proveedor = new ProveedorDAO();
                string l_s_Codigo= model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PROVEEDOR")), '0');

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID, "ProveedorBL.cs", "Insertar");

                l_s_Mensaje = l_dao_Proveedor.BuscarCodigo(l_s_Codigo, 0);
                if(l_s_Mensaje!="")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Proveedor.BuscarNombre(model.Nombre, 0);
                if (l_s_Mensaje != "")
                {
                    return l_s_Mensaje;
                }

                l_s_Mensaje = l_dao_Proveedor.Insertar(model);

                return l_s_Mensaje;

            }

            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorBL.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }
    }
}