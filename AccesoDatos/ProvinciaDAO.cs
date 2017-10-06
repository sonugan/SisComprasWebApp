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
    public class ProvinciaDAO
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

                l_s_stSql = "SELECT provincia_id";
                l_s_stSql += " FROM provincias";
                l_s_stSql += " WHERE UPPER(provincia_nombre) = '" + sNombre.ToUpper() + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND provincia_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProvinciaDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe una provincia con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "BuscarNombre");
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

                l_s_stSql = "SELECT provincia_id";
                l_s_stSql += " FROM provincias";
                l_s_stSql += " WHERE provincia_cod = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND provincia_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProvinciaDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe una provincia con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarProvincias()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProvinciaDAO.cs", "ConsultarProvincias");

                DataTable l_dt_Provincias = new DataTable();
                string l_s_stSql = "SELECT provincia_id, provincia_cod, provincia_nombre, flag_activo FROM provincias ORDER BY provincia_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProvinciaDAO.cs", "ConsultarProvincias");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Provincias = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Provincias.Fill(l_dt_Provincias);
                }
                return l_dt_Provincias;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "ConsultarProvincias");
                return null;
            }
            finally { }
        }

        public ProvinciaModel Consultar(int ProvinciaId)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + ProvinciaId.ToString(), "ProvinciaDAO.cs", "Consultar");

                DataTable l_dt_Provincia = new DataTable();
                ProvinciaModel l_model_Provincia = new ProvinciaModel();

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "SELECT provincia_id, provincia_cod, provincia_nombre, provincia_desc, flag_activo, numero_version FROM Provincias WHERE Provincia_id = ?";
                    OdbcDataAdapter l_da_Provincia = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Provincia.SelectCommand.Parameters.AddWithValue("id", ProvinciaId);
                    l_da_Provincia.Fill(l_dt_Provincia);

                }

                if (l_dt_Provincia.Rows.Count == 1)
                {
                    l_model_Provincia.ID = Convert.ToInt32(l_dt_Provincia.Rows[0]["provincia_id"].ToString());
                    l_model_Provincia.Codigo = l_dt_Provincia.Rows[0]["provincia_cod"].ToString();
                    l_model_Provincia.Nombre = l_dt_Provincia.Rows[0]["provincia_nombre"].ToString();
                    l_model_Provincia.Descripcion = l_dt_Provincia.Rows[0]["provincia_desc"].ToString();
                    l_model_Provincia.Activo = l_dt_Provincia.Rows[0]["flag_activo"].ToString();
                    l_model_Provincia.Version = Convert.ToInt32(l_dt_Provincia.Rows[0]["numero_version"].ToString());
                }

                return l_model_Provincia;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public string Insertar(ProvinciaModel p_model_Provincia)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProvinciaDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{? = CALL sp_provincias_insert (?, ?, ?, ?, ?)}";
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
                        if (p_model_Provincia.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Provincia.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Provincia.Codigo.ToUpper();
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Provincia.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Provincia.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Provincia.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Provincia.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Provincia.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Provincia.LoginCreacion;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ProvinciaModel p_model_Provincia)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProvinciaDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_provincias_update (?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Versión
                        OdbcParameter param = command.Parameters.Add("version", OdbcType.Int);
                        param.Value = p_model_Provincia.Version;
                        //Id
                        param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = p_model_Provincia.ID;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (p_model_Provincia.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Provincia.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Provincia.Codigo.ToUpper();
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Provincia.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Provincia.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Provincia.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Provincia.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Provincia.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Provincia.LoginUltModif;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Eliminar(int ProvinciaId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + ProvinciaId.ToString() + ", " + sUsuario, "ProvinciaDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_provincias_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = ProvinciaId;
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }
        public DataTable ConsultarProvinciasActivas(int iProvinciaId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iProvinciaId.ToString(), "ProvinciaDAO.cs", "ConsultarProvinciasActivas");

                DataTable l_dt_Provincias = new DataTable();
                string l_s_stSql = "SELECT provincia_id, provincia_cod, provincia_nombre";
                l_s_stSql += " FROM provincias";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if (iProvinciaId > 0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT provincia_id, provincia_cod, provincia_nombre";
                    l_s_stSql += " FROM provincias";
                    l_s_stSql += " WHERE provincia_id = " + iProvinciaId.ToString();
                }
                l_s_stSql += " ORDER BY 3";// Provincia_nombre
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProvinciaDAO.cs", "ConsultarProvinciasActivas");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Provincias = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Provincias.Fill(l_dt_Provincias);
                }
                return l_dt_Provincias;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaDAO.cs", "ConsultarProvinciasActivas");
                return null;
            }
            finally { }
        }

    }
}
