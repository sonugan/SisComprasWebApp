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
    public class DomicilioModel
    {
        public int DomicilioId { get; set; }
        [DisplayName("Localidad")]
        public int LocalidadId { get; set; }
        [DisplayName("Provincia")]
        public int ProvinciaId { get; set; }
        [DisplayName("País")]
        public int PaisId { get; set; }
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DisplayName("Cód. Postal")]
        public string CodigoPostal { get; set; }
        [DisplayName("Teléfono")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }
        [DisplayName("Fax")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Fax inválido")]
        public string Fax { get; set; }
        [DisplayName("Tél. móvil")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Teléfono móvil inválido")]
        public string TelCelular { get; set; }
        [DisplayName("eMail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Correo electrónico inválido")]
        public string eMail { get; set; }
        [DisplayName("Contacto")]
        public string Contacto { get; set; }
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [DisplayName("Tipo domicilio")]
        public string DomicilioTipoCod { get; set; }
        [DisplayName("Es primario?")]
        public string FlagPrimario { get; set; }

        public string ObjetoTipo { get; set; }
        public int ObjetoId { get; set; }

        public string LoginCreacion { get; set; }
        public string LoginUltModif { get; set; }

        public IEnumerable<SelectListItem> TiposDomicilio { get; set; }
        public IEnumerable<SelectListItem> LocalidadesActivas { get; set; }
        public IEnumerable<SelectListItem> ProvinciasActivas { get; set; }
        public IEnumerable<SelectListItem> PaisesActivos { get; set; }

    }
}
