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
    public class PaisDAO
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

                l_s_stSql = "SELECT pais_id";
                l_s_stSql += " FROM paises";
                l_s_stSql += " WHERE UPPER(pais_nombre) = '" + sNombre.ToUpper() + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND pais_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "PaisDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un país con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "BuscarNombre");
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

                l_s_stSql = "SELECT pais_id";
                l_s_stSql += " FROM paises";
                l_s_stSql += " WHERE pais_cod = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND pais_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "PaisDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un país con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string BuscarCodigoAFIP(string sCodigo, int iId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                string l_s_stSql = "";
                OdbcDataReader l_dr_Codigo;

                l_s_stSql = "SELECT pais_id";
                l_s_stSql += " FROM paises";
                l_s_stSql += " WHERE pais_destino_afip = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND pais_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "PaisDAO.cs", "BuscarCodigoAFIP");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un país con código AFIP " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "BuscarCodigoAFIP");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarPaises()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "PaisDAO.cs", "ConsultarPaises");

                DataTable l_dt_Paises = new DataTable();
                string l_s_stSql = "SELECT pais_id, pais_cod, pais_nombre, flag_activo, pais_destino_afip FROM paises ORDER BY pais_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "PaisDAO.cs", "ConsultarPaises");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Paises = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Paises.Fill(l_dt_Paises);
                }
                return l_dt_Paises;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "ConsultarPaises");
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + paisId.ToString(), "PaisDAO.cs", "Consultar");

                DataTable l_dt_Pais = new DataTable();
                PaisModel model = new PaisModel();

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "SELECT pais_id, pais_cod, pais_nombre, pais_desc, flag_activo, pais_destino_afip, numero_version FROM paises WHERE pais_id = ?";
                    OdbcDataAdapter l_da_Pais = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Pais.SelectCommand.Parameters.AddWithValue("id", paisId);
                    l_da_Pais.Fill(l_dt_Pais);

                }

                if (l_dt_Pais.Rows.Count == 1)
                {
                    model.ID = Convert.ToInt32(l_dt_Pais.Rows[0]["pais_id"].ToString());
                    model.Codigo = l_dt_Pais.Rows[0]["pais_cod"].ToString();
                    model.Nombre = l_dt_Pais.Rows[0]["pais_nombre"].ToString();
                    model.Descripcion = l_dt_Pais.Rows[0]["pais_desc"].ToString();
                    model.Activo = l_dt_Pais.Rows[0]["flag_activo"].ToString();
                    model.CodigoAFIP = l_dt_Pais.Rows[0]["pais_destino_afip"].ToString();
                    model.Version = Convert.ToInt32(l_dt_Pais.Rows[0]["numero_version"].ToString());
                }

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public string Insertar(PaisModel p_model_Pais)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "PaisDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{? = CALL sp_paises_insert (?, ?, ?, ?, ?, ?)}";
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
                        if (p_model_Pais.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PAIS")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Pais.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Pais.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Pais.Activo;
                        //Código de país según AFIP
                        param = command.Parameters.Add("cod_afip", OdbcType.VarChar, 5);
                        if (p_model_Pais.CodigoAFIP == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.CodigoAFIP == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.CodigoAFIP;
                            }
                        }
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Pais.LoginCreacion;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(PaisModel p_model_Pais)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "PaisDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_paises_update (?, ?, ?, ?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Versión
                        OdbcParameter param = command.Parameters.Add("version", OdbcType.Int);
                        param.Value = p_model_Pais.Version;
                        //Id
                        param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = p_model_Pais.ID;
                        //Código de país
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 10);
                        if (p_model_Pais.Codigo == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.Codigo == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PAIS")), '0');
                            }
                        }
                        //Nombre de país
                        param = command.Parameters.Add("nombre", OdbcType.VarChar, 80);
                        param.Value = l_dao_Generales.InitCap(p_model_Pais.Nombre);
                        //Descripción
                        param = command.Parameters.Add("descripcion", OdbcType.VarChar, 250);
                        if (p_model_Pais.Descripcion == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.Descripcion == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.Descripcion;
                            }
                        }
                        //Activo
                        param = command.Parameters.Add("activo", OdbcType.VarChar, 2);
                        param.Value = p_model_Pais.Activo;
                        //Código de país según AFIP
                        param = command.Parameters.Add("cod_afip", OdbcType.VarChar, 5);
                        if (p_model_Pais.CodigoAFIP == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            if (p_model_Pais.CodigoAFIP == "")
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                param.Value = p_model_Pais.CodigoAFIP;
                            }
                        }
                        //Usuario logueado
                        param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                        param.Value = p_model_Pais.LoginUltModif;

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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Eliminar(int PaisId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + PaisId.ToString() + ", " + sUsuario, "PaisDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_paises_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = PaisId;
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarPaisesActivos(int iPaisId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iPaisId.ToString(), "PaisDAO.cs", "ConsultarPaisesActivos");

                DataTable l_dt_Paises = new DataTable();
                string l_s_stSql = "SELECT pais_id, pais_cod, pais_nombre";
                l_s_stSql += " FROM paises";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if (iPaisId > 0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT pais_id, pais_cod, pais_nombre";
                    l_s_stSql += " FROM paises";
                    l_s_stSql += " WHERE pais_id = " + iPaisId.ToString();
                }
                l_s_stSql += " ORDER BY 3";// Pais_nombre
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "PaisDAO.cs", "ConsultarPaisesActivos");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Paises = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Paises.Fill(l_dt_Paises);
                }
                return l_dt_Paises;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisDAO.cs", "ConsultarPaisesActivos");
                return null;
            }
            finally { }
        }

    }
}
