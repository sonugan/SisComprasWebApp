using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using AccesoDatos;
using System.Data;
using Modelos.Dtos;

namespace SisCompras.BL
{
    public class OrdenCompraBL
    {
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

                return l_dao_OCompra.Insertar(ordenCompra);

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
            return (new OrdenCompraDAO()).ConsultarOrdenCompra(ordenCompraId);
        }

    }
}
