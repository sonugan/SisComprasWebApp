using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using AccesoDatos;
using System.Data;
using Modelos.Dtos;
using EnvioEmail;
using System.IO;
using System.Configuration;

namespace SisCompras.BL
{
    public class OrdenCompraBL
    {
        private OrdenCompraDAO ordenCompraDao;

        public OrdenCompraBL()
        {
            ordenCompraDao = new OrdenCompraDAO();
        }

        public List<OrdenCompraDto> ConsultarOrdenesCompra()
        {
            
            AplicacionLog.Logueo logger = new AplicacionLog.Logueo();
            string mensaje = "";

            try
            {
                OrdenCompraDAO ordenDeCompraDao = new OrdenCompraDAO();
                
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraBL.cs", "ConsultarOrdenesCompra");

                List<OrdenCompraDto> ordenesDeCompra = new List<OrdenCompraDto>();
                foreach(DataRow dr in ordenDeCompraDao.ConsultarOrdenesCompra().Rows)
                {
                    ordenesDeCompra.Add(new OrdenCompraDto()
                    {
                        Id = dr["orden_compra_cab_id"].ToString(),
                        CodigoProveedor = dr["proveedor_cod"].ToString(),
                        NombreProveedor = dr["proveedor_nombre"].ToString(),
                        FechaEmision = dr["fecha_emision"] != null ? Convert.ToDateTime(dr["fecha_emision"]) : new DateTime(),
                        NumeroReferencia = dr["numero_referencia"] != null ? dr["numero_referencia"].ToString() : "",
                        CantidadPedida = dr["cantidad_pedida_total"] != null ? Convert.ToInt32(dr["cantidad_pedida_total"]) : 0,
                        CodigoMonedaOpearcion = dr["moneda_operacion_cod"] != null ? dr["moneda_operacion_cod"].ToString() : "",
                        ImporteTotal = dr["importe_total"] != null ? Convert.ToDecimal(dr["importe_total"]) : 0,
                        Numero = dr["orden_compra_nro"] != null ? dr["orden_compra_nro"].ToString() : "",
                    });
                }
                return ordenesDeCompra;
            }
            catch (Exception miEx)
            {
                mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(mensaje);
                logger.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, mensaje, "OrdenCompraBL.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        public string InsertarCabecera(OrdenCompraModel ordenCompra)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                OrdenCompraDAO l_dao_OCompra = new OrdenCompraDAO();
                DataTable l_dt_OCompra = new DataTable();

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraBL.cs", "ConsultarOrdenesCompra");

                return l_dao_OCompra.InsertarCabecera(ordenCompra);

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraBL.cs", "ConsultarOrdenesCompra");
                return null;
            }
            finally { }
        }

        public OrdenCompraModel ConsultarOrdenCompra(int ordenCompraId)
        {
            return ordenCompraDao.ConsultarOrdenCompra(ordenCompraId);
        }

        public ArticuloOrdenCompraDto ConsultarArticulo(int ordenCompraId, int articuloId)
        {
            return ordenCompraDao.ConsultarArticulo(ordenCompraId, articuloId);
        }

        public string AgregarArticulo(ArticuloOrdenCompraDto articuloOrdenCompra)
        {
            return ordenCompraDao.AgregarArticulo(articuloOrdenCompra);
        }

        public ListaPaginada<ArticuloOrdenCompraDto> ConsultarArticulosOrdenCompra(Paginado paginado, int cabeceraId)
        {
            return ordenCompraDao.ConsultarArticulosOrdenCompra(paginado, cabeceraId);
        }

        public void Enviar(int ordenDeCompraId)
        {
            var ordenDeCompra = ConsultarOrdenCompra(ordenDeCompraId);
            ordenDeCompra.lineas = ordenCompraDao.ConsultarLineasOrdenCompra(ordenDeCompraId);

            Serializador<OrdenDeCompraSerializarDto> serializadorOrdenCompra = new Serializador<OrdenDeCompraSerializarDto>();

            var ordenCompraSerializada = serializadorOrdenCompra.SerializeToString(new OrdenDeCompraSerializarDto(ordenDeCompra));

            var direccionMailDesde = ConfigurationManager.AppSettings["DireccionMailDesde"];
            var direccionMailHasta = ConfigurationManager.AppSettings["DireccionMailHasta"];

            if(string.IsNullOrEmpty(direccionMailDesde) || string.IsNullOrEmpty(direccionMailHasta))
            {
                throw new Exception("No se encuentran configuradas correctamente las direcciones de envio de mail");
            }

            var email = new Email()
            {
                Asunto = "",
                Cc = "",
                Desde = direccionMailDesde,
                Para = direccionMailHasta
            };

            using (Stream s = GenerateStreamFromString(ordenCompraSerializada))
            {
                email.AgregarAdjunto(s);
            }

            var smtp = ConfigurationManager.AppSettings["smtp"];
            var puerto = ConfigurationManager.AppSettings["port"];

            if (string.IsNullOrEmpty(smtp) || string.IsNullOrEmpty(puerto))
            {
                throw new Exception("No se encuentra configurado correctamente el smtp o puerto para el envío de mails");
            }
            
            EmailSender.SendMail(smtp, puerto, email);

        }

        public void Guardar(OrdenCompraModel ordenDeCompra)
        {
            ordenCompraDao.Guardar(ordenDeCompra);
        }

        public void Eliminar(int id)
        {
            ordenCompraDao.Eliminar(id);
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
