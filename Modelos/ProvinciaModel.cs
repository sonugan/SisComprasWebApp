using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

//https://www.youtube.com/watch?v=1IFS33sPDhE
namespace Modelos
{
    public class ProvinciaModel
    {
        public int ID { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [DBColumnaDynamicLength("provincias", "provincia_cod")]
        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DBColumnaDynamicLength("provincias", "provincia_nombre")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [DBColumnaDynamicLength("provincias", "provincia_desc")]
        public string Descripcion { get; set; }

        [DisplayName("Activo")]
        public string Activo { get; set; }

        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }

        //[DisplayName("Precio")]
        //public decimal Price { get; set; }
        public IEnumerable<SelectListItem> Activos { get; set; }

    }
}