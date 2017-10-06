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
    public class ProveedorModel
    {
        public int ID { get; set; }

        //Para probar RegularExpression https://regex101.com/

        [DisplayName("Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El código debe ser numérico")]
        [CodigoDynamicLength("LARGO_CODIGO_PROVEEDOR", ErrorMessage = "El código debe ser de hasta {0} caracteres")]
        public string Codigo { get; set; }

        [DisplayName("Razón social")]
        [Required(ErrorMessage = "La razón social es obligatoria")]
        [DBColumnaDynamicLength("proveedores", "proveedor_nombre")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [DBColumnaDynamicLength("proveedores", "proveedor_desc")]
        public string Descripcion { get; set; }

        [DisplayName("Activo")]
        public string Activo { get; set; }

        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }

        //public int DomicilioId { get; set; }
        //[DisplayName("Localidad")]
        //public int LocalidadId { get; set; }
        //[DisplayName("Provincia")]
        //public int ProvinciaId { get; set; }
        //[DisplayName("País")]
        //public int PaisId { get; set; }
        //[DisplayName("Dirección")]
        //public string Direccion { get; set; }
        //[DisplayName("Cód. Postal")]
        //public string CodigoPostal { get; set; }
        //[DisplayName("Teléfono")]
        //[DataType(DataType.PhoneNumber,  ErrorMessage = "Teléfono inválido")]
        //public string Telefono { get; set; }
        //[DisplayName("Fax")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "Fax inválido")]
        //public string Fax { get; set; }
        //[DisplayName("Tél. móvil")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono móvil inválido")]
        //public string TelCelular { get; set; }
        //[DisplayName("eMail")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico inválido")]
        //public string eMail { get; set; }
        //[DisplayName("Contacto")]
        //public string Contacto { get; set; }
        //[DisplayName("Observaciones")]
        //public string ObsDomicilio { get; set; }
        //[DisplayName("Tipo domicilio")]
        //public string DomicilioTipoCod { get; set; }
        //[DisplayName("Es primario?")]
        //public string FlagPrimario { get; set; }
        public DomicilioModel domicilio { get; set; }

        public IEnumerable<SelectListItem> Activos { get; set; }

    }
}