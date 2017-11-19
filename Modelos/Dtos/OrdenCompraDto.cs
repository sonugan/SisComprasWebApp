using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Dtos
{
    public class OrdenCompraDto
    {
        public string Id { get; set; }
        public string CodigoProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroReferencia { get; set; }
        public int CantidadPedida { get; set; }
        public string CodigoMonedaOpearcion { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Numero { get; set; }
    }
}
