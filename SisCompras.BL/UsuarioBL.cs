using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using AccesoDatos;
using Modelos;

namespace SisCompras.BL
{
    public class UsuarioBusinessLogic
    {
        public string ValidarUsuarioIngreso(UsuarioModel objUsuario)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            UsuarioDAO l_dao_Usuarios = new UsuarioDAO();

            return l_dao_Usuarios.ValidarUsuarioIngreso(objUsuario);
        }
    }
}
