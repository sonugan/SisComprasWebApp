using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class ArticuloModel
    {
        public int ID { get; set; }
        public DateTime FechaCarga { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "articulo_cod")]
        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "articulo_nombre")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "articulo_desc")]
        public string Descripcion { get; set; }

        [DisplayName("Activo")]
        public string Activo { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProveedorId { get; set; }
        //public string ProveedorCod { get; set; }
        //[DisplayName("Proveedor")]
        //[Required(ErrorMessage = "El proveedor es obligatorio")]
        //public string ProveedorNombre { get; set; }

        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }

        [DisplayName("Moneda")]
        [Required(ErrorMessage = "La moneda es obligatoria")]
        public int MonedaId { get; set; }
        //public string MonedaCod { get; set; }

        [DisplayName("Cotización")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [CotizacionRequiredIf("MonedaId")]
        public decimal Cotizacion { get; set; }

        [DisplayName("Precio")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PrecioEnMoneda { get; set; }

        [DisplayName("Costo RMB")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostoRMB { get; set; }

        [DisplayName("Costo INNER")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostoINNER { get; set; }

        [DisplayName("Costo CBM")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostoCBM { get; set; }

        [DisplayName("Un. X bulto")]
        public Int16 UniXbulto { get; set; }

        [DisplayName("Peso bruto")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal PesoBruto { get; set; }
        [DisplayName("Peso neto")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal PesoNeto { get; set; }

        [DisplayName("Pendiente")]
        [ReadOnly(true)]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal PendienteRecep { get; set; }

        [DisplayName("Rubro")]
        public int RubroId { get; set; }
        //public string RubroCod { get; set; }
        //public string RubroNombre { get; set; }

        [DisplayName("Descripción 2")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "articulo_desc2")]
        public string Descripcion2 { get; set; }

        [DisplayName("Código de barras")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "codigo_barras")]
        public string CodigoBarras { get; set; }

        [DisplayName("Observaciones 1")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "observaciones_1")]
        public string Observaciones1 { get; set; }

        [DisplayName("Observaciones 2")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "observaciones_2")]
        public string Observaciones2 { get; set; }

        [DisplayName("Observaciones 3")]
        [DBColumnaDynamicLength("articulos_x_proveedores", "observaciones_3")]
        public string Observaciones3 { get; set; }

        public Foto Foto { get; set; }

        public IEnumerable<SelectListItem> Activos { get; set; }

        //https://www.codeproject.com/Questions/805945/how-to-fill-dropdownlist-from-database-without-usi
        //http://programmingwithrudra.blogspot.com.ar/2016/05/fill-dropdown-control-from-database-in.html
        //https://stackoverflow.com/questions/25205060/how-to-retrieve-image-from-database-without-using-entity-framework-in-asp-net-mv
        public IEnumerable<SelectListItem> ProveedoresActivos { get; set; }
        public IEnumerable<SelectListItem> MonedasActivas { get; set; }
        public IEnumerable<SelectListItem> RubrosActivos { get; set; }

    }

    public class CatalogoModel
    {
        [DisplayName("Código")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Codigo { get; set; }

        [DisplayName("Nombre")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Nombre { get; set; }

        [DisplayName("Descripción")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Descripcion { get; set; }

        [DisplayName("Descripción 2")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Descripcion2 { get; set; }
        
        [DisplayName("Moneda")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Moneda { get; set; }
        
        [DisplayName("Precio")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Precio { get; set; }
        
        [DisplayName("Cotización")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Cotizacion { get; set; }
        
        [DisplayName("Rubro")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Rubro { get; set; }
        
        [DisplayName("RMB")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 RMB { get; set; }
        
        [DisplayName("INNER")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 INNER { get; set; }
        
        [DisplayName("CBM")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 CBM { get; set; }
        
        [DisplayName("Un X bulto")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 UniXbulto { get; set; }
        
        [DisplayName("Peso neto")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 PesoNeto { get; set; }
        
        [DisplayName("Peso bruto")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 PesoBruto { get; set; }
        
        [DisplayName("Cód. barras")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 CodigoBarras { get; set; }
        
        [DisplayName("Obs. 1")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Observaciones1 { get; set; }
        
        [DisplayName("Obs. 2")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Observaciones2 { get; set; }
        
        [DisplayName("Obs. 3")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de columna debe ser numérico")]
        public Int16 Observaciones3 { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProveedorId { get; set; }

        [DisplayName("Moneda")]
        public int MonedaId { get; set; }

        public string LoginCreacion { get; set; }

        public IEnumerable<SelectListItem> ProveedoresActivos { get; set; }

        public System.Data.DataTable dtExcel { get; set; }

    }
}