using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Dtos
{
    public class ArticuloOrdenCompraDto
    {
        public int ID { get; set; }

        public int CabeceraId { get; set; }
        
        public int ArticuloId { get; set; }
        public string CodigoArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public int ProveedorId { get; set; }

        public Foto FotoArticulo { get; set; }

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

        public string LoginUltModif { get; set; }
    }
}
