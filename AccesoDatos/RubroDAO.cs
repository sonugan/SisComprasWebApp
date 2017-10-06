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
    public class RubroDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public int ObtenerId(string sRubroNombre)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int iRubroId = 0;

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + sRubroNombre, "RubroDAO.cs", "ObtenerId");

                string l_s_stSql = "";
                string sRubroSinAcentos = sRubroNombre.ToUpper().Replace('Á', '_').Replace('É','_').Replace('Í', '_').Replace('Ó', '_').Replace('Ú', '_').Replace('Ñ', '_');
                OdbcDataReader l_dr_Moneda;

                l_s_stSql = "SELECT rubro_id";
                l_s_stSql += " FROM rubros";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                //Tengo problemas con las letras acentuadas
                l_s_stSql += " AND UPPER(rubro_nombre) LIKE '" + sRubroSinAcentos + "'";

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "RubroDAO.cs", "ObtenerId");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Moneda = cmd.ExecuteReader();
                    if (l_dr_Moneda.HasRows)
                    {
                        iRubroId = Convert.ToInt32(l_dr_Moneda.GetValue(0));
                    }
                    cmd.Dispose();

                }

                return iRubroId;
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "ObtenerId");
                return -1;
            }
            finally { }
        }

        public string BuscarNombre(string sNombre, int iId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT rubro_id";
                l_s_stSql += " FROM rubros";
                l_s_stSql += " WHERE UPPER(rubro_nombre) = '" + sNombre.ToUpper() + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND rubro_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "RubroDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un rubro con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "BuscarNombre");
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

                l_s_stSql = "SELECT rubro_id";
                l_s_stSql += " FROM rubros";
                l_s_stSql += " WHERE rubro_cod = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND rubro_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "RubroDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un rubro con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarRubros()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "RubroDAO.cs", "ConsultarRubros");

                DataTable l_dt_Rubros = new DataTable();
                string l_s_stSql = "SELECT rubro_id, rubro_cod, rubro_nombre, flag_activo FROM rubros ORDER BY rubro_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "RubroDAO.cs", "ConsultarRubros");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Rubros = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Rubros.Fill(l_dt_Rubros);
                }
                return l_dt_Rubros;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "ConsultarRubros");
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + rubroId.ToString(), "RubroDAO.cs", "Consultar");

                DataTable l_dt_Rubro = new DataTable();
                RubroModel l_model_Rubro = new RubroModel();

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "SELECT rubro_id, rubro_cod, rubro_nombre, rubro_desc, flag_activo, numero_version FROM rubros WHERE rubro_id = ?";
                    OdbcDataAdapter l_da_Rubro = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Rubro.SelectCommand.Parameters.AddWithValue("id", rubroId);
                    l_da_Rubro.Fill(l_dt_Rubro);

                }

                if (l_dt_Rubro.Rows.Count == 1)
                {
                    l_model_Rubro.ID = Convert.ToInt32(l_dt_Rubro.Rows[0]["rubro_id"].ToString());
                    l_model_Rubro.Codigo = l_dt_Rubro.Rows[0]["rubro_cod"].ToString();
                    l_model_Rubro.Nombre = l_dt_Rubro.Rows[0]["rubro_nombre"].ToString();
                    l_model_Rubro.Descripcion = l_dt_Rubro.Rows[0]["rubro_desc"].ToString();
                    l_model_Rubro.Activo = l_dt_Rubro.Rows[0]["flag_activo"].ToString();
                    l_model_Rubro.Version = Convert.ToInt32(l_dt_Rubro.Rows[0]["numero_version"].ToString());
                }

                return l_model_Rubro;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public string Insertar(RubroModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "RubroDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{? = CALL sp_rubros_insert ( ?, ?, ?, ?, ?)}";
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
                        if (model.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_RUBRO")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(model.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (model.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = model.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = model.LoginCreacion;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(RubroModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "RubroDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_rubros_update ( ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Versión
                        OdbcParameter param = command.Parameters.Add("version", OdbcType.Int);
                        param.Value = model.Version;
                        //Id
                        param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = model.ID;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (model.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_RUBRO")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(model.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (model.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (model.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = model.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = model.Activo;
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = model.LoginUltModif;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Eliminar(int RubroId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + RubroId.ToString() + ", " + sUsuario, "RubroDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_rubros_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = RubroId;
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarRubrosActivos(int iRubroId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iRubroId.ToString(), "RubroDAO.cs", "ConsultarRubrosActivos");

                DataTable l_dt_Rubros = new DataTable();
                string l_s_stSql = "SELECT rubro_id, rubro_cod, rubro_nombre";
                l_s_stSql += " FROM rubros";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if (iRubroId > 0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT rubro_id, rubro_cod, rubro_nombre";
                    l_s_stSql += " FROM rubros";
                    l_s_stSql += " WHERE rubro_id = " + iRubroId.ToString();
                }
                l_s_stSql += " ORDER BY 3";// rubro_nombre
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "RubroDAO.cs", "ConsultarRubrosActivos");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Rubros = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Rubros.Fill(l_dt_Rubros);
                }
                return l_dt_Rubros;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroDAO.cs", "ConsultarRubrosActivos");
                return null;
            }
            finally { }
        }

    }
}
