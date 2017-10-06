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
    public class OrdenCompraModel
    {

        public OCCabeceraModel cabecera { get; set; }
        public List<OCLineaModel> lineas { get; set; }

    }

    //Cabecera de la orden de compra
    public class OCCabeceraModel
    {

        public int ID { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número debe ser numérico")]
        [CodigoDynamicLength("LARGO_NUMERO_ORDENCOMPRA", ErrorMessage = "El número debe ser de hasta {0} caracteres")]
        public string Numero { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProveedorId { get; set; }

        [DisplayName("Observaciones")]
        [DBColumnaDynamicLength("ordenes_compra_cab", "observaciones")]
        public string Observaciones { get; set; }

        [DisplayName("Referencia")]
        [DBColumnaDynamicLength("ordenes_compra_cab", "numero_referencia")]
        public string NroReferencia { get; set; }

        [DisplayName("Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de emisión es obligatoria")]
        public DateTime FechaEmision { get; set; }

        [DisplayName("Condición de compra")]
        public int CondicionCompraId { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CantidadTotal { get; set; }

        [DisplayName("Recibido")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RecibidoTotal { get; set; }

        [DisplayName("Importe")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal ImporteTotal { get; set; }

        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }

        [DisplayName("Moneda")]
        [Required(ErrorMessage = "La moneda es obligatoria")]
        public int MonedaOperacionId { get; set; }

        public int MonedaNacionalId { get; set; }

        [DisplayName("Cotización")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [CotizacionRequiredIf("MonedaOperacionId")]
        public decimal Cotizacion { get; set; }

        public IEnumerable<SelectListItem> ProveedoresActivos { get; set; }
        public IEnumerable<SelectListItem> MonedasActivas { get; set; }

    }

    //Línea de la orden de compra
    public class OCLineaModel
    {
        public int ID { get; set; }
        public int CabeceraId { get; set; }

        [DisplayName("Artículo")]
        [Required(ErrorMessage = "El artículo es obligatorio")]
        public int ArticuloXProveedorId { get; set; }

        [DisplayName("Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Cantidad { get; set; }

        [DisplayName("Recibido")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Recibido { get; set; }

        [DisplayName("Precio")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Precio { get; set; }

        [DisplayName("Recepción")]
        public DateTime FechaRecepcion { get; set; }

        [DisplayName("% Dto")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PorcDescuento { get; set; }

        [DisplayName("Un. Medida")]
        public string UnidadMedida { get; set; }

        public IEnumerable<SelectListItem> ArticulosActivos { get; set; }
    }
}
