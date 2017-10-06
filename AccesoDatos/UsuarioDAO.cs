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
    public class UsuarioDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

        public string ValidarUsuarioIngreso(UsuarioModel objUsuario)
        {
            string sMensaje = "";
            string sContrasena = "";

            try
            {
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string l_s_stSql = "";
                    OdbcTransaction trx = connection.BeginTransaction();

                    l_s_stSql = "SELECT contrasena FROM sistema_usuarios";
                    l_s_stSql += " WHERE login = ?";
                    l_s_stSql += " AND flag_activo = ?";

                    using (OdbcCommand command = new OdbcCommand(l_s_stSql, connection, trx))
                    {
                        command.Transaction = trx;
                        command.CommandType = CommandType.Text;

                        //Login
                        command.Parameters.Add(new OdbcParameter("login", objUsuario.Usuario.ToUpper()));
                        //Activo
                        command.Parameters.Add(new OdbcParameter("login", "Si"));

                        OdbcDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                                sContrasena = (dr.GetString(0));
                            if (objUsuario.Contrasena != sContrasena)
                            {
                                sMensaje = "Contraseña incorrecta. Intente nuevamente.";
                            }
                        }
                        else
                        {
                            sMensaje = "Usuario no existe o no está activo";
                        }
                        trx.Commit();
                    }
                }

                return sMensaje;

            }
            catch (Exception miEx)
            {
                sMensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(sMensaje);
                return sMensaje;
            }
        }
    }
}
