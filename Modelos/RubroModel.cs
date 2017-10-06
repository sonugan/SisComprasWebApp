using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class RubroModel
    {
        public int ID { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El código debe ser numérico")]
        [CodigoDynamicLength("LARGO_CODIGO_RUBRO", ErrorMessage = "El código debe ser de hasta {0} caracteres")]
        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DBColumnaDynamicLength("rubros", "rubro_nombre")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [DBColumnaDynamicLength("rubros", "rubro_desc")]
        public string Descripcion { get; set; }

        [DisplayName("Activo")]
        public string Activo { get; set; }

        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }

        public IEnumerable<SelectListItem> Activos { get; set; }
    }
}
