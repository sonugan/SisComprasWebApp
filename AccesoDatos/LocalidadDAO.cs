using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using Modelos;

namespace AccesoDatos
{
    public class LocalidadDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public string BuscarNombre(string sNombre, int iId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT localidad_id";
                l_s_stSql += " FROM localidades";
                l_s_stSql += " WHERE UPPER(localidad_nombre) = '" + sNombre.ToUpper() + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND localidad_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "LocalidadDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe una localidad con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "BuscarNombre");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string BuscarCodigo(string sCodigo, int iId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT localidad_id";
                l_s_stSql += " FROM localidades";
                l_s_stSql += " WHERE localidad_cod = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND localidad_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "LocalidadDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe una localidad con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarLocalidades()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "LocalidadDAO.cs", "ConsultarLocalidades");

                DataTable l_dt_Localidades = new DataTable();
                string l_s_stSql = "SELECT localidad_id, localidad_cod, localidad_nombre, flag_activo FROM localidades ORDER BY localidad_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "LocalidadDAO.cs", "ConsultarLocalidades");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Localidades = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Localidades.Fill(l_dt_Localidades);
                }
                return l_dt_Localidades;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "ConsultarLocalidades");
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + localidadId.ToString(), "LocalidadDAO.cs", "Consultar");

                DataTable l_dt_Localidad = new DataTable();
                LocalidadModel model = new LocalidadModel();

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "SELECT localidad_id, localidad_cod, localidad_nombre, localidad_desc, flag_activo, numero_version FROM localidades WHERE localidad_id = ?";
                    OdbcDataAdapter l_da_Localidad = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Localidad.SelectCommand.Parameters.AddWithValue("id", localidadId);
                    l_da_Localidad.Fill(l_dt_Localidad);

                }

                if (l_dt_Localidad.Rows.Count == 1)
                {
                    model.ID = Convert.ToInt32(l_dt_Localidad.Rows[0]["localidad_id"].ToString());
                    model.Codigo = l_dt_Localidad.Rows[0]["localidad_cod"].ToString();
                    model.Nombre = l_dt_Localidad.Rows[0]["localidad_nombre"].ToString();
                    model.Descripcion = l_dt_Localidad.Rows[0]["localidad_desc"].ToString();
                    model.Activo = l_dt_Localidad.Rows[0]["flag_activo"].ToString();
                    model.Version = Convert.ToInt32(l_dt_Localidad.Rows[0]["numero_version"].ToString());
                }

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public string Insertar(LocalidadModel p_model_Localidad)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "LocalidadDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{? = CALL sp_localidades_insert (?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                        param.Direction = ParameterDirection.ReturnValue;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (p_model_Localidad.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Localidad.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Localidad.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_LOCALIDAD")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Localidad.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Localidad.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Localidad.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Localidad.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Localidad.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Localidad.LoginCreacion;

                        OdbcDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                            l_s_Mensaje = (dr.GetString(0));
                        trx.Commit();
                    }

                }

                return l_s_Mensaje; //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(LocalidadModel p_model_Localidad)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "LocalidadDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_Localidades_update (?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Versión
                        OdbcParameter param = command.Parameters.Add("version", OdbcType.Int);
                        param.Value = p_model_Localidad.Version;
                        //Id
                        param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = p_model_Localidad.ID;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (p_model_Localidad.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Localidad.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Localidad.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_LOCALIDAD")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Localidad.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Localidad.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Localidad.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Localidad.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Localidad.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Localidad.LoginUltModif;

                        command.ExecuteNonQuery();
                        trx.Commit();
                    }

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Eliminar(int LocalidadId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + LocalidadId.ToString() + ", " + sUsuario, "LocalidadDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_localidades_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = LocalidadId;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = sUsuario;

                        command.ExecuteNonQuery();
                        trx.Commit();
                    }

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarLocalidadesActivas(int iLocalidadId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iLocalidadId.ToString(), "LocalidadDAO.cs", "ConsultarLocalidadesActivas");

                DataTable l_dt_Localidades = new DataTable();
                string l_s_stSql = "SELECT localidad_id, localidad_cod, localidad_nombre";
                l_s_stSql += " FROM localidades";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if (iLocalidadId > 0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT localidad_id, localidad_cod, localidad_nombre";
                    l_s_stSql += " FROM localidades";
                    l_s_stSql += " WHERE localidad_id = " + iLocalidadId.ToString();
                }
                l_s_stSql += " ORDER BY 3";// Localidad_nombre
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "LocalidadDAO.cs", "ConsultarLocalidadesActivas");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Localidades = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Localidades.Fill(l_dt_Localidades);
                }
                return l_dt_Localidades;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadDAO.cs", "ConsultarLocalidadesActivas");
                return null;
            }
            finally { }
        }

    }
}
