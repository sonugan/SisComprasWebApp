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
    public class PaisModel
    {
        public int ID { get; set; }

        [DisplayName("Código")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "99999999")]
        //[DataType(DataType.PhoneNumber,  ErrorMessage = "Not a number")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El código debe ser numérico")]
        [CodigoDynamicLength("LARGO_CODIGO_PAIS", ErrorMessage = "El código debe ser de hasta {0} caracteres")]
        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DBColumnaDynamicLength("paises", "pais_nombre")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [DBColumnaDynamicLength("paises", "pais_desc")]
        public string Descripcion { get; set; }

        [DisplayName("Activo")]
        public string Activo { get; set; }

        [DisplayName("Código AFIP")]
        [DBColumnaDynamicLength("paises", "pais_destino_afip")]
        public string CodigoAFIP { get; set; }

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