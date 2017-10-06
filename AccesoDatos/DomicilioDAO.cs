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
    public class DomicilioDAO
    {

        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public string Eliminar(int domicilioId, string sUsuario, OdbcConnection connection, OdbcTransaction trx)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + domicilioId.ToString(), "DomicilioDAO.cs", "Eliminar");

                string l_s_stSql = "{ CALL sp_domicilios_delete (?, ?) }";

                using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                {
                    command.Transaction = trx;
                    command.CommandType = CommandType.StoredProcedure;

                    //Id
                    OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                    param.Value = domicilioId;
                    //Usuario logueado
                    param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                    param.Value = sUsuario;

                    command.ExecuteNonQuery();

                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Domicilio eliminado: " + domicilioId.ToString(), "DomicilioDAO.cs", "Eliminar");

                }

                return l_s_Mensaje; //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "DomicilioDAO.cs", "Eliminar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Actualizar(DomicilioModel model, OdbcConnection connection, OdbcTransaction trx)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "DomicilioDAO.cs", "Actualizar");

                string l_s_stSql = "{? = CALL sp_domicilios_update (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";

                using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                {
                    command.Transaction = trx;
                    command.CommandType = CommandType.StoredProcedure;

                    //Id (salida)
                    OdbcParameter param = command.Parameters.Add("Id_out", OdbcType.Int);
                    param.Direction = ParameterDirection.ReturnValue;
                    //Id (entrada)
                    param = command.Parameters.Add("Id_in", OdbcType.Int);
                    param.Value = model.DomicilioId;
                    //Objeto tipo
                    param = command.Parameters.Add("objeto_tipo", OdbcType.VarChar, 50);
                    if (model.ObjetoTipo == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.ObjetoTipo == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.ObjetoTipo.ToUpper();
                        }
                    }
                    //Objeto Id
                    param = command.Parameters.Add("objeto_id", OdbcType.Int);
                    if (model.ObjetoId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.ObjetoId;
                    }
                    //Dirección
                    param = command.Parameters.Add("direccion", OdbcType.VarChar, 200);
                    if (model.Direccion == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Direccion == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = l_dao_Generales.InitCap(model.Direccion);
                        }
                    }
                    //Código postal
                    param = command.Parameters.Add("codigo_postal", OdbcType.VarChar, 10);
                    if (model.CodigoPostal == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.CodigoPostal == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.CodigoPostal;
                        }
                    }
                    //Localidad Id
                    param = command.Parameters.Add("localidad_id", OdbcType.Int);
                    if (model.LocalidadId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.LocalidadId;
                    }
                    //Provincia Id
                    param = command.Parameters.Add("provincia_id", OdbcType.Int);
                    if (model.ProvinciaId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.ProvinciaId;
                    }
                    //Teléfono
                    param = command.Parameters.Add("telefono", OdbcType.VarChar, 150);
                    if (model.Telefono == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Telefono == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Telefono;
                        }
                    }
                    //Fax
                    param = command.Parameters.Add("fax", OdbcType.VarChar, 10);
                    if (model.Fax == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Fax == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Fax;
                        }
                    }
                    //Teléfono celular
                    param = command.Parameters.Add("tel_celular", OdbcType.VarChar, 10);
                    if (model.TelCelular == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.TelCelular == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.TelCelular;
                        }
                    }
                    //eMail
                    param = command.Parameters.Add("email", OdbcType.VarChar, 10);
                    if (model.eMail == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.eMail == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.eMail;
                        }
                    }
                    //Tipo de domicilio
                    param = command.Parameters.Add("tipo", OdbcType.VarChar, 50);
                    if (model.DomicilioTipoCod == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.DomicilioTipoCod == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.DomicilioTipoCod;
                        }
                    }
                    //Es primiario?
                    param = command.Parameters.Add("primario", OdbcType.VarChar, 2);
                    if (model.FlagPrimario == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.FlagPrimario == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.FlagPrimario;
                        }
                    }
                    //Contacto
                    param = command.Parameters.Add("contacto", OdbcType.VarChar, 250);
                    if (model.Contacto == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Contacto == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Contacto;
                        }
                    }
                    //País Id
                    param = command.Parameters.Add("pais_id", OdbcType.Int);
                    if (model.PaisId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.PaisId;
                    }
                    //Observaciones
                    param = command.Parameters.Add("observaciones", OdbcType.VarChar, 250);
                    if (model.Observaciones == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Observaciones == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Observaciones;
                        }
                    }
                    //Usuario logueado
                    param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                    param.Value = model.LoginCreacion;

                    OdbcDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                        l_s_Mensaje = (dr.GetString(0));

                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Domicilio actualizado/insertado: " + l_s_Mensaje, "DomicilioDAO.cs", "Actualizar");

                }

                return l_s_Mensaje; //devuelve el Id generado o el Id actualizado
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "DomicilioDAO.cs", "Actualizar");
                return l_s_Mensaje;
            }
            finally { }
        }

        public string Insertar(DomicilioModel model, OdbcConnection connection, OdbcTransaction trx)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            GeneralesDAO l_dao_Generales = new GeneralesDAO();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "DomicilioDAO.cs", "Insertar");

                string l_s_stSql = "{? = CALL sp_domicilios_insert (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";

                using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                {
                    command.Transaction = trx;
                    command.CommandType = CommandType.StoredProcedure;

                    //Id
                    OdbcParameter param = command.Parameters.Add("Id", OdbcType.Int);
                    param.Direction = ParameterDirection.ReturnValue;
                    //Objeto tipo
                    param = command.Parameters.Add("objeto_tipo", OdbcType.VarChar, 50);
                    if (model.ObjetoTipo == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.ObjetoTipo == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.ObjetoTipo.ToUpper();
                        }
                    }
                    //Objeto Id
                    param = command.Parameters.Add("objeto_id", OdbcType.Int);
                    if (model.ObjetoId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.ObjetoId;
                    }
                    //Dirección
                    param = command.Parameters.Add("direccion", OdbcType.VarChar, 200);
                    if (model.Direccion == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Direccion == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = l_dao_Generales.InitCap(model.Direccion);
                        }
                    }
                    //Código postal
                    param = command.Parameters.Add("codigo_postal", OdbcType.VarChar, 10);
                    if (model.CodigoPostal == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.CodigoPostal == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.CodigoPostal;
                        }
                    }
                    //Localidad Id
                    param = command.Parameters.Add("localidad_id", OdbcType.Int);
                    if (model.LocalidadId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.LocalidadId;
                    }
                    //Provincia Id
                    param = command.Parameters.Add("provincia_id", OdbcType.Int);
                    if (model.ProvinciaId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.ProvinciaId;
                    }
                    //Teléfono
                    param = command.Parameters.Add("telefono", OdbcType.VarChar, 150);
                    if (model.Telefono == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Telefono == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Telefono;
                        }
                    }
                    //Fax
                    param = command.Parameters.Add("fax", OdbcType.VarChar, 10);
                    if (model.Fax == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Fax == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Fax;
                        }
                    }
                    //Teléfono celular
                    param = command.Parameters.Add("tel_celular", OdbcType.VarChar, 10);
                    if (model.TelCelular == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.TelCelular == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.TelCelular;
                        }
                    }
                    //eMail
                    param = command.Parameters.Add("email", OdbcType.VarChar, 10);
                    if (model.eMail == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.eMail == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.eMail;
                        }
                    }
                    //Código postal
                    param = command.Parameters.Add("tipo", OdbcType.VarChar, 50);
                    if (model.DomicilioTipoCod == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.DomicilioTipoCod == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.DomicilioTipoCod;
                        }
                    }
                    //Es primiario?
                    param = command.Parameters.Add("primario", OdbcType.VarChar, 2);
                    if (model.FlagPrimario == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.FlagPrimario == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.FlagPrimario;
                        }
                    }
                    //Contacto
                    param = command.Parameters.Add("contacto", OdbcType.VarChar, 250);
                    if (model.Contacto == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Contacto == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Contacto;
                        }
                    }
                    //País Id
                    param = command.Parameters.Add("pais_id", OdbcType.Int);
                    if (model.PaisId <= 0)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = model.PaisId;
                    }
                    //Observaciones
                    param = command.Parameters.Add("observaciones", OdbcType.VarChar, 250);
                    if (model.Observaciones == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        if (model.Observaciones == "")
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = model.Observaciones;
                        }
                    }
                    //Usuario logueado
                    param = command.Parameters.Add("login", OdbcType.VarChar, 50);
                    param.Value = model.LoginCreacion;

                    OdbcDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                        l_s_Mensaje = (dr.GetString(0));

                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Domicilio insertado: " + l_s_Mensaje, "DomicilioDAO.cs", "Insertar");

                }

                return l_s_Mensaje; //devuelve el Id
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "DomicilioDAO.cs", "Insertar");
                return l_s_Mensaje;
            }
            finally { }
        }

    }
}
