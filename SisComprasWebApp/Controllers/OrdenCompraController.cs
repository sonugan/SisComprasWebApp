using System;
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
        // GET: OrdenCompra
        [HttpGet]
        public ActionResult Index()
        {
            OrdenCompraBL l_bl_OCompras = new OrdenCompraBL();
            DataTable l_dt_OCompras = new DataTable();

            l_dt_OCompras = l_bl_OCompras.ConsultarOrdenesCompra();

            return View(l_dt_OCompras);
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
                var cabecera = new OCCabeceraModel();

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

        // POST: OrdenCompra/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdenCompra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
            return View();
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
                List<ArticuloModel> articulos = l_bl_Articulo.ConsultarArticulosCarga(sProveedorId, sFechaCarga);

                return View(articulos);
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
        public ActionResult ConsultarArticulosCargados(string sProveedorId, string sFechaCarga)
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

                var articulos = l_bl_Articulo.ConsultarArticulosCarga(sProveedorId, sFechaCarga)
                    .Select(a => new
                    {
                        Codigo = a.Codigo,
                        Nombre = a.Nombre,
                        Descripcion = a.Descripcion,
                        Foto = "\\<img src='data:image/jpg;base64," + a.Foto.ToBase64 + "' style='height:150px; width:150px'\\>"
                    });

                //return Json(articulos, JsonRequestBehavior.AllowGet);

                return Json(
                    new
                    {
                        data = articulos
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
