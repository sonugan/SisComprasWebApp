using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class UsuarioModel
    {
        public int ID { get; set; }

        [DisplayName("Usuario")]
        [Required]
        public string Usuario { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Contraseña")]
        [Required]
        public string Contrasena { get; set; }

        public int GrupoDefaultId { get; set; }
        public string GrupoDefaultNombre { get; set; }

    }
}