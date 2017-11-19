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
    public class ProveedorDAO
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

                l_s_stSql = "SELECT proveedor_id";
                l_s_stSql += " FROM proveedores";
                l_s_stSql += " WHERE UPPER(proveedor_nombre) = '" + sNombre.ToUpper() + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND proveedor_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProveedorDAO.cs", "BuscarNombre");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if (l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un proveedor con nombre " + sNombre;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "BuscarNombre");
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

                l_s_stSql = "SELECT proveedor_id";
                l_s_stSql += " FROM proveedores";
                l_s_stSql += " WHERE proveedor_cod = '" + sCodigo + "'";

                if (iId > 0)
                {
                    l_s_stSql += " AND proveedor_id <> " + iId.ToString();
                }

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProveedorDAO.cs", "BuscarCodigo");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                    l_dr_Codigo = cmd.ExecuteReader();
                    if(l_dr_Codigo.HasRows)
                    {
                        l_s_Mensaje = "Ya existe un proveedor con código " + sCodigo;
                    }
                    cmd.Dispose();

                }

                return l_s_Mensaje;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "BuscarCodigo");
                return l_s_Mensaje;
            }
            finally { }
        }

        public DataTable ConsultarProveedores()
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorDAO.cs", "ConsultarProveedores");

                DataTable l_dt_Proveedores = new DataTable();
                string l_s_stSql = "SELECT proveedor_id, proveedor_cod, proveedor_nombre, flag_activo FROM proveedores ORDER BY proveedor_cod";
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProveedorDAO.cs", "ConsultarProveedores");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Proveedores = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Proveedores.Fill(l_dt_Proveedores);
                }
                return l_dt_Proveedores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "ConsultarProveedores");
                return null;
            }
            finally { }
        }

        public List<ProveedorModel> ConsultarProveedoresActivos()
        {
            List<ProveedorModel> proveedores = new List<ProveedorModel>();

            var tablaProveedores = ConsultarProveedoresActivos(0);

            foreach (DataRow dr in tablaProveedores.Rows)
            {
                ProveedorModel proveedor = new ProveedorModel()
                {
                    ID = Convert.ToInt32(dr["proveedor_id"]),
                    Codigo = dr["proveedor_cod"].ToString(),
                    Nombre = dr["proveedor_nombre"].ToString(),
                    //Descripcion = dr["proveedor_dec"] != null ? dr["proveedor_desc"].ToString() : null,
                    //Activo = dr["flag_activo"] != null ? dr["flag_activo"].ToString() : "No",
                    //Version = dr["numero_version"] != null ? Convert.ToInt32(dr["numero_version"]) : 0,
                    //FechaCreacion = Convert.ToDateTime(dr["fecha_creacion"]),
                    //LoginCreacion = dr["login_creacion"] != null ? dr["login_creacion"].ToString() : null,
                    //FechaUltModif = dr["fecha_ult_modif"] != null ? Convert.ToDateTime(dr["login_ult_modif"]) : new DateTime(),
                    //LoginUltModif = dr["login_ult_modif"] != null ? dr["login_ult_modif"].ToString() : null,
                };
                proveedores.Add(proveedor);
            }

            return proveedores;
        }

        public DataTable ConsultarProveedoresActivos(int iProveedorId)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + iProveedorId.ToString(), "ProveedorDAO.cs", "ConsultarProveedoresActivos");

                DataTable l_dt_Proveedores = new DataTable();
                string l_s_stSql = "SELECT proveedor_id, proveedor_cod, proveedor_nombre";
                l_s_stSql += " FROM proveedores";
                l_s_stSql += " WHERE flag_activo = 'Si'";
                if(iProveedorId>0)
                {
                    l_s_stSql += " UNION";
                    l_s_stSql += " SELECT proveedor_id, proveedor_cod, proveedor_nombre";
                    l_s_stSql += " FROM proveedores";
                    l_s_stSql += " WHERE proveedor_id = " + iProveedorId.ToString();
                }
                l_s_stSql += " ORDER BY 3";// proveedor_nombre
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProveedorDAO.cs", "ConsultarProveedoresActivos");

                using (OdbcConnection odbcConn = new OdbcConnection(connectionString))
                {
                    odbcConn.Open();
                    OdbcDataAdapter l_da_Proveedores = new OdbcDataAdapter(l_s_stSql, odbcConn);
                    l_da_Proveedores.Fill(l_dt_Proveedores);
                }
                return l_dt_Proveedores;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "ConsultarProveedoresActivos");
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
                DataTable l_dt_Proveedor = new DataTable();
                ProveedorModel model = new ProveedorModel();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + proveedorId.ToString(), "ProveedorDAO.cs", "Consultar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    string l_s_stSql = "SELECT p.proveedor_id, p.proveedor_cod, p.proveedor_nombre, p.proveedor_desc, p.flag_activo, p.numero_version";
                    l_s_stSql += ",d.direccion, d.codigo_postal, d.localidad_id, d.provincia_id, d.pais_id, d.flag_domicilio_tipo_primario";
                    l_s_stSql += ",d.telefono, d.fax, d.tel_celular, d.email, d.contacto, d.observaciones, d.domicilio_tipo_cod, d.domicilio_id";
                    l_s_stSql += ",d.objeto_id, d.objeto_tipo";
                    l_s_stSql += " FROM proveedores p";
                    l_s_stSql += " LEFT JOIN sp_domicilios_consulta_principal('PROVEEDORES', p.proveedor_id) dp";
                    l_s_stSql += " ON dp.po_v_ObjetoTipo = 'PROVEEDORES' AND dp.po_i_ObjetoId = p.proveedor_id";
                    l_s_stSql += " LEFT JOIN vw_domicilios d";
                    l_s_stSql += " ON dp.po_i_DomicilioId = d.domicilio_id";
                    l_s_stSql += " WHERE p.proveedor_id = ? ";
                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_stSql, "ProveedorDAO.cs", "Consultar");

                    connection.Open();
                    OdbcDataAdapter l_da_Proveedor = new OdbcDataAdapter(l_s_stSql, connection);
                    l_da_Proveedor.SelectCommand.Parameters.AddWithValue("id", proveedorId);
                    l_da_Proveedor.Fill(l_dt_Proveedor);

                }

                if (l_dt_Proveedor.Rows.Count == 1)
                {
                    model.ID = Convert.ToInt32(l_dt_Proveedor.Rows[0]["proveedor_id"].ToString());
                    model.Codigo = l_dt_Proveedor.Rows[0]["proveedor_cod"].ToString();
                    model.Nombre = l_dt_Proveedor.Rows[0]["proveedor_nombre"].ToString();
                    model.Descripcion = l_dt_Proveedor.Rows[0]["proveedor_desc"].ToString();
                    model.Activo = l_dt_Proveedor.Rows[0]["flag_activo"].ToString();
                    model.Version = Convert.ToInt32(l_dt_Proveedor.Rows[0]["numero_version"].ToString());
                    //Domicilio
                    model.domicilio = new DomicilioModel();//¿Está bien manejado esto así?
                    model.domicilio.CodigoPostal = l_dt_Proveedor.Rows[0]["codigo_postal"].ToString();
                    model.domicilio.Contacto = l_dt_Proveedor.Rows[0]["contacto"].ToString();
                    model.domicilio.Direccion = l_dt_Proveedor.Rows[0]["direccion"].ToString();
                    model.domicilio.DomicilioId = (l_dt_Proveedor.Rows[0]["domicilio_id"].ToString() == "") ? 0 : Convert.ToInt32(l_dt_Proveedor.Rows[0]["domicilio_id"].ToString());
                    model.domicilio.DomicilioTipoCod = l_dt_Proveedor.Rows[0]["domicilio_tipo_cod"].ToString();
                    model.domicilio.eMail = l_dt_Proveedor.Rows[0]["email"].ToString();
                    model.domicilio.Fax = l_dt_Proveedor.Rows[0]["fax"].ToString();
                    model.domicilio.FlagPrimario = l_dt_Proveedor.Rows[0]["flag_domicilio_tipo_primario"].ToString();
                    model.domicilio.LocalidadId = (l_dt_Proveedor.Rows[0]["localidad_id"].ToString() == "") ? 0 : Convert.ToInt32(l_dt_Proveedor.Rows[0]["localidad_id"].ToString());
                    model.domicilio.ObjetoId = (l_dt_Proveedor.Rows[0]["objeto_id"].ToString() == "") ? 0 : Convert.ToInt32(l_dt_Proveedor.Rows[0]["objeto_id"].ToString());
                    model.domicilio.ObjetoTipo = l_dt_Proveedor.Rows[0]["objeto_tipo"].ToString();
                    model.domicilio.Observaciones = l_dt_Proveedor.Rows[0]["observaciones"].ToString();
                    model.domicilio.PaisId = (l_dt_Proveedor.Rows[0]["localidad_id"].ToString() == "") ? 0 : Convert.ToInt32(l_dt_Proveedor.Rows[0]["pais_id"].ToString());
                    model.domicilio.ProvinciaId = (l_dt_Proveedor.Rows[0]["provincia_id"].ToString() == "") ? 0 : Convert.ToInt32(l_dt_Proveedor.Rows[0]["provincia_id"].ToString());
                    model.domicilio.TelCelular = l_dt_Proveedor.Rows[0]["tel_celular"].ToString();
                    model.domicilio.Telefono = l_dt_Proveedor.Rows[0]["telefono"].ToString();
                }

                return model;

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "Consultar");
                return null;
            }
            finally { }
        }

        public string Insertar(ProveedorModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                int l_i_Id = 0;
                int l_i_Resultado = 0;
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorDAO.cs", "Insertar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{? = CALL sp_proveedores_insert (?, ?, ?, ?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                        param.Direction = ParameterDirection.ReturnValue;
                        //Código de proveedor
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 20);
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
                                param.Value = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PROVEEDOR")), '0');
                            }
                        }
                        //Nombre de proveedor
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
                        {
                            l_i_Id = dr.GetInt32(0);
                        }

                        l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Proveedor insertado: " + l_i_Id.ToString(), "ProveedorDAO.cs", "Insertar");

                        if(model.domicilio != null)
                        {
                            model.domicilio.ObjetoId = l_i_Id;
                            model.domicilio.ObjetoTipo = "PROVEEDORES";
                            DomicilioDAO domicilioDAO = new DomicilioDAO();

                            l_s_Mensaje = domicilioDAO.Insertar(model.domicilio, connection, trx);
                            if(l_s_Mensaje != "")
                            {
                                if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID del domicilio generado
                                { }
                                else
                                {
                                    trx.Rollback();
                                    System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_Mensaje, "ProveedorDAO.cs", "Insertar");
                                    return l_s_Mensaje;
                                }
                            }
                        }

                        trx.Commit();

                    }

                }

                return l_i_Id.ToString(); //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(ProveedorModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorDAO.cs", "Actualizar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_proveedores_update (?, ?, ?, ?, ?, ?, ?)}";
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
                        //Código de proveedor
                        param = command.Parameters.Add("codigo", OdbcType.VarChar, 20);
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
                                param.Value = model.Codigo.PadLeft(Convert.ToInt32(l_dao_Generales.ObtenerOpcionSistema("LARGO_CODIGO_PROVEEDOR")), '0');
                            }
                        }
                        //Nombre de proveedor
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

                        if(model.domicilio != null)
                        {
                            model.domicilio.ObjetoId = model.ID;
                            model.domicilio.ObjetoTipo = "PROVEEDORES";
                            DomicilioDAO domicilioDAO = new DomicilioDAO();

                            l_s_Mensaje = domicilioDAO.Actualizar(model.domicilio, connection, trx);
                            if (l_s_Mensaje != "")
                            {
                                if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID del domicilio generado
                                { }
                                else
                                {
                                    trx.Rollback();
                                    System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, l_s_Mensaje, "ProveedorDAO.cs", "Insertar");
                                    return l_s_Mensaje;
                                }
                            }
                        }

                        trx.Commit();
                    }

                }

                return "";

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "Actualizar");
                return l_s_Mensaje ;
            }
            finally { }
        }

        public string Eliminar(int proveedorId, string sUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + proveedorId.ToString() + ", " + sUsuario, "ProveedorDAO.cs", "Eliminar");

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "{ CALL sp_proveedores_delete (?, ?)}";
                    OdbcTransaction trx = connection.BeginTransaction();

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //Id
                        OdbcParameter param = command.Parameters.Add("id", OdbcType.Int);
                        param.Value = proveedorId;
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally {  }
        }
    }
}
