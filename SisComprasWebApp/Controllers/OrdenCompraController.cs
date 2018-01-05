﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelos;
using System.Data;
using SisCompras.BL;
using System.Configuration;
using SisComprasWebApp.Models;
using Modelos.Dtos;

namespace SisComprasWebApp.Controllers
{

    //http://zahidadeel.blogspot.com.ar/2011/05/master-detail-form-in-aspnet-mvc-3-ii.html#!/2011/05/master-detail-form-in-aspnet-mvc-3-ii.html
    //https://www.youtube.com/watch?v=ir9cMbNQP4w (usa entity framework)
    //https://www.youtube.com/watch?v=ir9cMbNQP4w
    //http://demo.aspnetawesome.com/MasterDetailCrudDemo
    //http://www.codeproject.com/Articles/531916/Master-Details-using-ASP-NET-MVC
    //https://www.mindstick.com/Articles/1117/crud-operation-using-modal-dialog-in-asp-dot-net-mvc

    //Sin entity framework
    //http://www.codesolution.org/asp-net-mvc-crud-operations/

    public class OrdenCompraController : Controller
    {
        private OrdenCompraBL ordenDeCompraBl;
        private ProveedorBL proveedorBl;
        private MonedaBL monedaBl;

        public OrdenCompraController()
        {
            ordenDeCompraBl = new OrdenCompraBL();
            proveedorBl = new ProveedorBL();
            monedaBl = new MonedaBL();
        }

        // GET: OrdenCompra
        [HttpGet]
        public ActionResult Index()
        {
            var ordenesDeCompra = ordenDeCompraBl.ConsultarOrdenesCompra();

            return View(ordenesDeCompra);
        }

        // GET: OrdenCompra/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdenCompra/Create
        public ActionResult Create()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                var model = new OrdenCompraModel();
                var cabecera = new OCCabeceraModel()
                {
                    FechaEmision = DateTime.Now
                };
                cabecera.Numero = OCCabeceraModel.ObtenerNumero();
                Orden = null;
                //DropDownList de proveedores
                ProveedorBL l_bl_Proveedor = new ProveedorBL();
                DataTable l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedoresActivos(0);
                List<SelectListItem> l_sli_Proveedores = new List<SelectListItem>();
                foreach (DataRow fila in l_dt_Proveedores.Rows)
                {
                    l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString() });
                }
                //model.ProveedoresActivos = l_sli_Proveedores;
                cabecera.ProveedoresActivos = l_sli_Proveedores;

                //DropDownList de monedas
                MonedaBL l_bl_Moneda = new MonedaBL();
                DataTable l_dt_Monedas = l_bl_Moneda.ConsultarMonedasActivas(0);
                List<SelectListItem> l_sli_Monedas = new List<SelectListItem>();
                foreach (DataRow fila in l_dt_Monedas.Rows)
                {
                    if (fila["flag_default"].ToString() == "Si")
                    {
                        l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString(), Selected = true });
                    }
                    else
                    {
                        l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString() });
                    }
                }
                cabecera.MonedasActivas = l_sli_Monedas;

                model.cabecera = cabecera;
                Orden = new OrdenCompraModel() { cabecera = new OCCabeceraModel(), lineas = new List<OCLineaModel>() };
                ViewBag.RecargarGrilla = "false";
                return View(model);
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "Create [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Orden de Compra";
                return View("Error");
            }
            finally { }
        }

        public ActionResult Edit(int? ordenCompraId)
        {
            try
            {
                var ordenCompra = Orden;
                if (ordenCompraId.HasValue)
                {
                    ordenCompra = ordenDeCompraBl.ConsultarOrdenCompra(ordenCompraId.Value);
                    Orden = ordenCompra;
                }
                else
                {
                    Orden.cabecera.ID = int.MaxValue;
                }
                ordenCompra.cabecera.ProveedoresActivos = proveedorBl.ConsultarProveedoresActivos()
                       .Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Nombre });

                ordenCompra.cabecera.MonedasActivas = monedaBl.ConsultarMonedasActivasList(0)
                    .Select(m => new SelectListItem { Value = m.ID.ToString(), Text = m.Codigo.ToString(), Selected = m.FlagDefault == "Si" });
                ViewBag.RecargarGrilla = "false";
                return View("Create", ordenCompra);
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: OrdenCompra/Create
        [HttpPost]
        public ActionResult Create(OrdenCompraModel ordenDeCompra)
        {
            try
            {
                if (ordenDeCompra.cabecera.ID != 0)
                {
                    ViewBag.RecargarGrilla = "true";
                }
                else
                {
                    ViewBag.RecargarGrilla = "false";
                }
                if (ModelState.IsValid)
                {
                    ordenDeCompra.cabecera.LoginCreacion = Session["UsuarioLogueado"].ToString();
                    ordenDeCompra.cabecera.LoginUltModif = Session["UsuarioLogueado"].ToString();
                    //ordenDeCompraBl.InsertarCabecera(ordenDeCompra);
                    Orden = ordenDeCompra;//Agrego en la sesion
                    return RedirectToAction("Edit");
                }
                else
                {
                    ordenDeCompra.cabecera.ProveedoresActivos = proveedorBl.ConsultarProveedoresActivos()
                        .Select( p => new SelectListItem { Value = p.ID.ToString(), Text = p.Nombre });

                    ordenDeCompra.cabecera.MonedasActivas = monedaBl.ConsultarMonedasActivasList(0)
                        .Select(m => new SelectListItem { Value = m.ID.ToString(), Text = m.Codigo.ToString(), Selected = m.FlagDefault == "Si" });

                    return View(ordenDeCompra);
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: OrdenCompra/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdenCompra/Delete/5
        public ActionResult Delete(int id)
        {
            ordenDeCompraBl.Eliminar(id);
            var ordenesDeCompra = ordenDeCompraBl.ConsultarOrdenesCompra();
            return View("Index", ordenesDeCompra);
        }

        // POST: OrdenCompra/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult OCLineasCarga()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");

                string sProveedorId = Request.Form["FechaCarga"].ToString();
                string sFechaCarga = Request.Form["FechaCarga"].ToString();
                if (sFechaCarga == "")
                {
                    l_s_Mensaje = "No se ingresó la fecha de carga";
                    System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");
                    ViewBag.ErrorMessage = l_s_Mensaje;
                    ViewBag.ErrorObject = "Carga de líneas";
                    return View("Error");

                }

                ArticuloBL l_bl_Articulo = new ArticuloBL();
                ListaPaginada<ArticuloModel> articulos = l_bl_Articulo.ConsultarArticulosCarga(new Paginado(), sProveedorId, sFechaCarga);

                return View(articulos.Lista);
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "OCLineas";
                return View("Error");
            }
            finally { }
        }

        [HttpGet]
        public ActionResult ArticulosCargados(int cabeceraId)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                return View(new OrdenCompraModel() { cabecera = new OCCabeceraModel() { ID = cabeceraId } });
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "OCLineas";
                return View("Error");
            }
            finally { }
        }

        [HttpGet]
        public ActionResult ConsultarArticulosCargadosEnOrden(int cabeceraId)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");

                ArticuloBL l_bl_Articulo = new ArticuloBL();
                var articulos = l_bl_Articulo.ConsultarArticulosCarga(new Paginado(), Orden.cabecera.ProveedorId.ToString(), "");

                var cargados = Orden.lineas
                    .Join(articulos.Lista, l => l.ArticuloXProveedorId, a => a.ID, (c, a) => new { Cargado = c, Articulo = a })
                    .Select(a => new ArticuloOrdenCompraDto()
                    {
                        ID = a.Cargado.ID,
                        Cantidad = a.Cargado.Cantidad,
                        CabeceraId = cabeceraId,
                        FechaRecepcion = a.Cargado.FechaRecepcion,
                        PorcDescuento = a.Cargado.PorcDescuento,
                        Precio = a.Cargado.Precio,
                        Recibido = a.Cargado.Recibido,
                        UnidadMedida = a.Cargado.UnidadMedida,
                        ArticuloId = a.Articulo.ID,
                        CodigoArticulo = a.Articulo.Codigo,
                        DescripcionArticulo = a.Articulo.Descripcion,
                        FotoArticulo = a.Articulo.Foto,
                        NombreArticulo = a.Articulo.Nombre,
                        ProveedorId = a.Articulo.ProveedorId
                    });
                    
                ListaPaginada<ArticuloOrdenCompraDto> articulosCargados = Paginar<ArticuloOrdenCompraDto>(cargados, new Paginado());
                    //ordenDeCompraBl.ConsultarArticulosOrdenCompra(new Paginado(), cabeceraId);
                return Json(
                   new
                   {
                       initialPage = articulos.Paginado.PaginaInicial,
                       pageSize = articulos.Paginado.TamanioHoja,
                       recordsTotal = articulos.Paginado.CantidadDeRegistros,
                       recordsFiltered = articulos.Paginado.RegistrosFiltrados,
                       data = articulosCargados.Lista.Select(a => new
                       {
                           ID = a.ID,
                           Codigo = a.CodigoArticulo,
                           Nombre = a.NombreArticulo,
                           Descripcion = a.DescripcionArticulo,
                           Cantidad = a.Cantidad,
                           Precio = a.Precio,
                           Foto = @"\<img class='productImage' src='data:image/jpg;base64," + a.FotoArticulo.ToBase64 + "' style='height:150px; width:150px'\\>",
                       })
                   }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "OCLineasCarga [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "OCLineas";
                return View("Error");
            }
            finally { }
        }
        
        [HttpGet]
        public ActionResult ConsultarArticulosCargados(int? start, int? length, string sProveedorId, string sFechaCargaDesde, string sFechaCargaHasta)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            //if (ModelState.IsValid)
            //{
            //    sMensaje = ublUsuario.ValidarUsuarioIngreso(pUsuario);
            //}
            //else
            //{
            //    var message = string.Join(" | ", ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage));
            //    sMensaje = "Error: " + message.ToString();
            //}

            //Session["UsuarioLogueado"] = "";

            //if (Request.IsAjaxRequest())
            //{
            //    if (sMensaje == "") //usuario logueado OK
            //    {
            //        Session["UsuarioLogueado"] = pUsuario.Usuario.ToUpper();
            //    }
            //    return Json(sMensaje, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "OrdenCompraController.cs", "ConsultarArticulosCargados [HttpGet]");

                ArticuloBL l_bl_Articulo = new ArticuloBL();
                Paginado paginado = new Paginado() { PaginaInicial = start, TamanioHoja = length };
                var articulos = l_bl_Articulo.ConsultarArticulosCarga(paginado, sProveedorId, sFechaCargaDesde, sFechaCargaHasta);

                //return Json(articulos, JsonRequestBehavior.AllowGet);

                return Json(
                    new
                    {
                        initialPage = articulos.Paginado.PaginaInicial,
                        pageSize = articulos.Paginado.TamanioHoja,
                        recordsTotal = articulos.Paginado.CantidadDeRegistros,
                        recordsFiltered = articulos.Paginado.RegistrosFiltrados,
                        data = articulos.Lista.Select(a => new
                        {
                            ID = a.ID,
                            Codigo = a.Codigo,
                            Nombre = a.Nombre,
                            Descripcion = a.Descripcion,
                            Foto = @"\<img src='data:image/jpg;base64," + a.Foto.ToBase64 + "' style='height:150px; width:150px'\\>",
                        })
                    }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "OrdenCompraController.cs", "ConsultarArticulosCargados [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "ConsultarArticulosCargados";
                return View("Error");
            }
            finally { }
        }


        [HttpGet]
        public ActionResult AddArticulo(int articuloId, int ordenDeCompraId)
        {
            var articulo = ordenDeCompraBl.ConsultarArticulo(articuloId, ordenDeCompraId);
            
            articulo.LoginUltModif = Session["UsuarioLogueado"].ToString();
            return View(articulo);
        }

        [HttpPost]
        public ActionResult AddArticulo(ArticuloOrdenCompraDto articulo)
        {
            if (ModelState.IsValid)
            {
                articulo.LoginUltModif = Session["UsuarioLogueado"].ToString();
                //ordenDeCompraBl.AgregarArticulo(articulo);
                this.AgregarArticulo(articulo);

                var ordenDeCompra = Orden;//ordenDeCompraBl.ConsultarOrdenCompra(articulo.CabeceraId);
                ordenDeCompra.cabecera.ProveedoresActivos = proveedorBl.ConsultarProveedoresActivos()
                      .Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Nombre });

                ordenDeCompra.cabecera.MonedasActivas = monedaBl.ConsultarMonedasActivasList(0)
                    .Select(m => new SelectListItem { Value = m.ID.ToString(), Text = m.Codigo.ToString(), Selected = m.FlagDefault == "Si" });
                ViewBag.RecargarGrilla = "true";
                ViewBag.DeshabilitarProveedores = "false";
                Orden.cabecera.ProveedorId = articulo.ProveedorId;
                return View("Create", ordenDeCompra);
            }
            else
            {
                return View(articulo);
            }
        }

        [HttpGet]
        public void EliminarArticulo(int articuloId)
        {
            var articulo = Orden.lineas.Where(l => l.ID == articuloId).FirstOrDefault();
            Orden.lineas.Remove(articulo);
        }

        [HttpGet]
        public ActionResult Enviar(int ordenDeCompraId)
        {
            try
            {
                ordenDeCompraBl.Enviar(ordenDeCompraId);
                return Content("ok");
            }
            catch(Exception ex)
            {
                return Content("fail");
            }
        }

        [HttpGet]
        public ActionResult Guardar()
        {
            try
            {
                ordenDeCompraBl.Guardar(Orden);
                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        public OrdenCompraModel Orden
        {
            get
            {
                return (OrdenCompraModel)Session["OrdenActual"];
            }
            set
            {
                Session["OrdenActual"] = value;
            }
        }

        private void AgregarArticulo(ArticuloOrdenCompraDto articulo)
        {
            if (Orden.lineas == null)
            {
                Orden.lineas = new List<OCLineaModel>();
            }
            var yaCargado = Orden.lineas.Where(l => l.ID == articulo.ID).FirstOrDefault();
            if (yaCargado != null)
            {
                yaCargado.Cantidad += articulo.Cantidad;
            }
            else
            {
                Orden.lineas.Add(new OCLineaModel()
                {
                    ID = articulo.ID,
                    Cantidad = articulo.Cantidad,
                    CabeceraId = articulo.CabeceraId,
                    FechaRecepcion = articulo.FechaRecepcion,
                    PorcDescuento = articulo.PorcDescuento,
                    Precio = articulo.Precio,
                    Recibido = articulo.Recibido,
                    UnidadMedida = articulo.UnidadMedida,
                    ArticuloXProveedorId = articulo.ArticuloId
                });
            }
        }

        public ListaPaginada<T> Paginar<T>(IEnumerable<T> lista, Paginado paginado) where T : class
        {
            int paginaInicial = paginado.PaginaInicial.HasValue ? paginado.PaginaInicial.Value : 0;
            int tamanioHoja = paginado.TamanioHoja.HasValue ? paginado.TamanioHoja.Value : lista.Count();

            var listaPaginada = new ListaPaginada<T>();
            listaPaginada.Paginado = paginado;
            listaPaginada.Lista = lista
                .Skip(paginaInicial * tamanioHoja)
                .Take(tamanioHoja)
                .ToList();

            return listaPaginada;
        }
    
        //public JsonResult CustomServerSideSearchAction(DataTableAjaxPostModel model)
        //{
        //    // action inside a standard controller
        //    int filteredResultsCount;
        //    int totalResultsCount;
        //    var res = new List<ArticuloModel>();//YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);

        //    return Json(new
        //    {
        //        // this is what datatables wants sending back
        //        draw = model.draw,
        //        recordsTotal = 10,//totalResultsCount,
        //        recordsFiltered = 10,// filteredResultsCount,
        //        data = res// result
        //    });
        //}
    }
}
