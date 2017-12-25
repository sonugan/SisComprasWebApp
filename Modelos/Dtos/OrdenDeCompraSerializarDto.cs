using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Dtos
{
    public class OrdenDeCompraSerializarDto
    {
        public OrdenDeCompraSerializarDto()
        {

        }
        public OrdenDeCompraSerializarDto(OrdenCompraModel ordenCompra)
        {
            Cabecera = new CabeceraOrdenDeCompraSerializarDto(ordenCompra.cabecera);
            Lineas = ordenCompra.lineas.Select(l => new LineaOrdenDeCompraSerialziarDto(l)).ToList();
        }
        public CabeceraOrdenDeCompraSerializarDto Cabecera { get; set; }
        public List<LineaOrdenDeCompraSerialziarDto> Lineas { get; set; }
    }

    public class CabeceraOrdenDeCompraSerializarDto
    {
        public CabeceraOrdenDeCompraSerializarDto()
        {

        }
        public CabeceraOrdenDeCompraSerializarDto(OCCabeceraModel cabecera)
        {
            ID = cabecera.ID;
            Numero = cabecera.Numero;
            ProveedorId = cabecera.ProveedorId;
            Observaciones = cabecera.Observaciones;
            NroReferencia = cabecera.NroReferencia;
            FechaEmision = cabecera.FechaEmision;
            CondicionCompraId = cabecera.CondicionCompraId;
            Estado = cabecera.Estado;
            CantidadTotal = cabecera.CantidadTotal;
            RecibidoTotal = cabecera.RecibidoTotal;
            ImporteTotal = cabecera.ImporteTotal;
            Version = cabecera.Version;
            LoginCreacion = cabecera.LoginCreacion;
            FechaCreacion = cabecera.FechaCreacion;
            LoginUltModif = cabecera.LoginUltModif;
            FechaUltModif = cabecera.FechaUltModif;
            MonedaOperacionId = cabecera.MonedaOperacionId;
            MonedaNacionalId = cabecera.MonedaNacionalId;
            Cotizacion = cabecera.Cotizacion;
        }

        public int ID { get; set; }
        public string Numero { get; set; }
        public int ProveedorId { get; set; }
        public string Observaciones { get; set; }
        public string NroReferencia { get; set; }
        public DateTime FechaEmision { get; set; }
        public int CondicionCompraId { get; set; }
        public string Estado { get; set; }
        public decimal CantidadTotal { get; set; }
        public decimal RecibidoTotal { get; set; }
        public decimal ImporteTotal { get; set; }
        public Int32 Version { get; set; }
        public string LoginCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string LoginUltModif { get; set; }
        public DateTime FechaUltModif { get; set; }
        public int MonedaOperacionId { get; set; }
        public int MonedaNacionalId { get; set; }
        public decimal Cotizacion { get; set; }
    }

    public class LineaOrdenDeCompraSerialziarDto
    {
        public LineaOrdenDeCompraSerialziarDto()
        {

        }
        public LineaOrdenDeCompraSerialziarDto(OCLineaModel linea)
        {
            ID = linea.ID;
            CabeceraId = linea.CabeceraId;
            ArticuloXProveedorId = linea.ArticuloXProveedorId;
            Cantidad = linea.Cantidad;
            Recibido = linea.Recibido;
            Precio = linea.Precio;
            FechaRecepcion = linea.FechaRecepcion;
            PorcDescuento = linea.PorcDescuento;
            UnidadMedida = linea.UnidadMedida;
        }
        public int ID { get; set; }
        public int CabeceraId { get; set; }
        public int ArticuloXProveedorId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Recibido { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public decimal PorcDescuento { get; set; }
        public string UnidadMedida { get; set; }
    }
}
