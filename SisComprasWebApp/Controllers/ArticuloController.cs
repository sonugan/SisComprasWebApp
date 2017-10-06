using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelos;
using System.Data;
using SisCompras.BL;
using System.Configuration;

namespace SisComprasWebApp.Controllers
{
    public class ArticuloController : Controller
    {
        // GET: Articulo
        [HttpGet]
        public ActionResult Index()
        {
            ArticuloBL l_bl_Articulo = new ArticuloBL();
            DataTable l_dt_Articulos = new DataTable();

            l_dt_Articulos = l_bl_Articulo.ConsultarArticulos();

            return View(l_dt_Articulos);
        }

        // GET: Articulo/Create
        [HttpGet]
        public ActionResult Create()
        {

            var model = new ArticuloModel();

            //DropDownList Si/No
            model.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //DropDownList de proveedores
            ProveedorBL l_bl_Proveedor = new ProveedorBL();
            DataTable l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedoresActivos(0);
            List<SelectListItem> l_sli_Proveedores = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Proveedores.Rows)
            {
                l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString() });
            }
            model.ProveedoresActivos = l_sli_Proveedores;

            //DropDownList de rubros
            RubroBL l_bl_Rubro = new RubroBL();
            DataTable l_dt_Rubros = l_bl_Rubro.ConsultarRubrosActivos(0);
            List<SelectListItem> l_sli_Rubros = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Rubros.Rows)
            {
                l_sli_Rubros.Add(new SelectListItem { Value = fila["rubro_id"].ToString(), Text = fila["rubro_nombre"].ToString() });
            }
            model.RubrosActivos = l_sli_Rubros;

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
            model.MonedasActivas = l_sli_Monedas;

            //return View(new ArticuloModel());
            return View(model);
        }

        // POST: Articulo/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
        //                            https://www.youtube.com/watch?v=1IFS33sPDhE
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpPost]
        public ActionResult Create(ArticuloModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloController.cs", "Crete [HttpPost]");

                //Puta madre!! no me anda nada de esto
                //http://techfunda.com/howto/259/passing-error-to-view-from-controller-action

                //Diseño de la vista con bootstrap:
                //https://www.jose-aguilar.com/scripts/css/bootstrap/3.1.1/grids.php

                string sUsuario = Session["UsuarioLogueado"].ToString();
                model.LoginCreacion = sUsuario;
                //Esto lo tenía cuando en el modelo tenía el código/nombre en lugar del Id
                //model.MonedaId = Convert.ToInt32(Request.Form["MonedaCod"].ToString());
                //model.ProveedorId = Convert.ToInt32(Request.Form["ProveedorNombre"].ToString());

                if (ModelState.IsValid)
                {
                    ArticuloBL l_bl_Articulo = new ArticuloBL();
                    l_s_Mensaje = l_bl_Articulo.Insertar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID del artículo generado
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = l_s_Mensaje;
                            ViewBag.ErrorObject = "Artículo";
                            //ModelState.AddModelError("", "Please write first name.");
                            return View("Error");
                        }
                    }
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMessage = "Los datos ingresados no son válidos: " + message.ToString();
                    ViewBag.ErrorObject = "Artículo";
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "Create [HttpPost]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Artículo";
                return View("Error");
            }
            finally { }
        }

        // GET: Articulo/Edit/5
        public ActionResult Edit(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + id.ToString(), "ArticuloController.cs", "Edit [HttpGet]");

                ArticuloModel model = new ArticuloModel();
                ArticuloBL l_bl_Articulo = new ArticuloBL();

                model = l_bl_Articulo.Consultar(id);

                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Activos = new List<SelectListItem> {
                    new SelectListItem { Value = "Si", Text = "Si"},
                    new SelectListItem { Value = "No", Text = "No"}
                    };

                    //DropDownList de proveedores
                    ProveedorBL l_bl_Proveedor = new ProveedorBL();
                    DataTable l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedoresActivos(model.ProveedorId);
                    List<SelectListItem> l_sli_Proveedores = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Proveedores.Rows)
                    {
                        //if (fila["proveedor_id"].ToString() == model.ProveedorId.ToString())
                        //{
                        //    l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString(), Selected = true });
                        //}
                        //else
                        //{
                            l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString() });
                        //}
                    }
                    model.ProveedoresActivos = l_sli_Proveedores;

                    //DropDownList de rubros
                    RubroBL l_bl_Rubro = new RubroBL();
                    DataTable l_dt_Rubros = l_bl_Rubro.ConsultarRubrosActivos(model.RubroId);
                    List<SelectListItem> l_sli_Rubros = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Rubros.Rows)
                    {
                        //if (fila["rubro_id"].ToString() == model.RubroId.ToString())
                        //{
                        //    l_sli_Rubros.Add(new SelectListItem { Value = fila["rubro_id"].ToString(), Text = fila["rubro_nombre"].ToString(), Selected = true });
                        //}
                        //else
                        //{
                            l_sli_Rubros.Add(new SelectListItem { Value = fila["rubro_id"].ToString(), Text = fila["rubro_nombre"].ToString() });
                        //}
                    }
                    model.RubrosActivos = l_sli_Rubros;

                    //DropDownList de monedas
                    MonedaBL l_bl_Moneda = new MonedaBL();
                    DataTable l_dt_Monedas = l_bl_Moneda.ConsultarMonedasActivas(0);// (model.MonedaId);
                    List<SelectListItem> l_sli_Monedas = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Monedas.Rows)
                    {
                        //if (fila["moneda_id"].ToString() == model.MonedaId.ToString())
                        //{
                        //    l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString(), Selected = true });
                        //}
                        //else
                        //{
                            l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString() });
                        //}
                    }
                    model.MonedasActivas = l_sli_Monedas;

                    return View(model);
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "Edit");
                //return View();
                return RedirectToAction("Index");
            }
            finally { }
        }

        // POST: Articulo/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
        //                                                    https://www.youtube.com/watch?v=1IFS33sPDhE
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpPost]
        public ActionResult Edit(ArticuloModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID.ToString(), "ArticuloController.cs", "Edit [HttpPost]");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                model.LoginUltModif = sUsuario;

                //Esto lo tenía cuando en el modelo tenía el código/nombre en lugar del Id
                //model.MonedaId = Convert.ToInt32(Request.Form["MonedaCod"].ToString());
                //model.ProveedorId = Convert.ToInt32(Request.Form["ProveedorNombre"].ToString());

                if (ModelState.IsValid)
                {
                    ArticuloBL l_bl_Articulo = new ArticuloBL();
                    l_s_Mensaje = l_bl_Articulo.Actualizar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        ViewBag.ErrorObject = "Artículo";
                        //ModelState.AddModelError("", "Please write first name.");
                        return View("Error");
                    }

                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMessage = "Los datos ingresados no son válidos: " + message.ToString();
                    ViewBag.ErrorObject = "Artículo";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "Edit [HttpPost]");
                return View();
            }
            finally { }
        }

        //GET: /Articulo/MostrarImagen/5
        public ActionResult MostrarImagen(int id)
        {

            return View();

        }

        // GET: Articulo/Delete/5
        public ActionResult Delete(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + id.ToString(), "ArticuloController.cs", "Delete [HttpGet]");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                ArticuloBL l_bl_Articulo = new ArticuloBL();
                l_s_Mensaje = l_bl_Articulo.Eliminar(id, sUsuario);

                return RedirectToAction("Index");
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "Delete");
                return View();
            }
            finally { }
        }

        //// POST: Articulo/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Articulo/UpLoad
        //[HttpGet]
        //public ActionResult UpLoad()
        //{

        //    //https://www.codeproject.com/Tips/615776/WebGrid-in-ASP-NET-MVC

        //    //No funciona!
        //    //http://www.mitechdev.com/2016/10/How-to-read-and-display-excelfile-in-mvc-webapplication.html

        //    //https://social.technet.microsoft.com/wiki/contents/articles/31790.asp-net-mvc-upload-read-excel-file.aspx

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpLoad(HttpPostedFileBase uploadfile)
        //{

        //    //https://www.codeproject.com/Tips/615776/WebGrid-in-ASP-NET-MVC

        //    //No funciona!
        //    //http://www.mitechdev.com/2016/10/How-to-read-and-display-excelfile-in-mvc-webapplication.html

        //    //https://social.technet.microsoft.com/wiki/contents/articles/31790.asp-net-mvc-upload-read-excel-file.aspx

        //    return View();
        //}

        [HttpGet]
        public ActionResult UpLoad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpLoad(FormCollection formCollection)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloController.cs", "UpLoad");

                if (Request == null)
                {
                    ViewBag.ErrorMessage = "Error al seleccionar archivo";
                    ViewBag.ErrorObject = "Catálogo";
                    return View("Error");
                }

                ExcelImportBL excel = new ExcelImportBL();
                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    DataSet ds = excel.ImportExcelXLS(file,false);//lo mando como que no tiene títulos
                    DataTable dt = ds.Tables[0];
                    GeneralesBL generalesBL = new GeneralesBL();
                    string sArchivo = ConfigurationManager.AppSettings["DirectorioTemporal"].ToString() + generalesBL.RandomNumber().ToString() + "_" + file.FileName;

                    file.SaveAs(sArchivo); // guardo el archivo en el server
                    Session["UploadedFile"] = sArchivo;//guardo el nombre del archivo en la sesión
                    l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Archivo guardado: " + sArchivo, "ArticuloController.cs", "UpLoad");

                    //DropDownList de proveedores: cargar antes de retonar la view  
                    //porque si no me da error:
                    //There is no ViewData item of type 'IEnumerable<SelectListItem>' that has the key 'ddlProveedor'
                    ProveedorBL l_bl_Proveedor = new ProveedorBL();
                    DataTable l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedoresActivos(0);
                    List<SelectListItem> l_sli_Proveedores = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Proveedores.Rows)
                    {
                        l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString() });
                    }
                    ViewBag.Proveedores = l_sli_Proveedores;

                    //https://stackoverflow.com/questions/2632715/retrieve-pictures-from-excel-file-using-oledb 

                    //DropDownList de monedas
                    MonedaBL l_bl_Moneda = new MonedaBL();
                    DataTable l_dt_Monedas = l_bl_Moneda.ConsultarMonedasActivas(0);// (model.MonedaId);
                    List<SelectListItem> l_sli_Monedas = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Monedas.Rows)
                    {
                        //if (fila["moneda_id"].ToString() == model.MonedaId.ToString())
                        //{
                        //    l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString(), Selected = true });
                        //}
                        //else
                        //{
                        l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString() });
                        //}
                    }
                    ViewBag.Monedas = l_sli_Monedas;

                    return View(dt);
                }
                else
                {
                    ViewBag.ErrorMessage = "Imposible cargar el archivo seleccionado";
                    ViewBag.ErrorObject = "Catálogo";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "UpLoad");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Catálogo";
                return View("Error");
            }
            finally { }
        }

        [HttpGet]
        public ActionResult UploadCatalogo()
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloController.cs", "UploadCatalogo [HttpGet]");

                //DropDownList de proveedores: cargar antes de retonar la view
                //porque si no me da error:
                //There is no ViewData item of type 'IEnumerable<SelectListItem>' that has the key 'ddlProveedor'
                ProveedorBL l_bl_Proveedor = new ProveedorBL();
                DataTable l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedoresActivos(0);
                List<SelectListItem> l_sli_Proveedores = new List<SelectListItem>();
                foreach (DataRow fila in l_dt_Proveedores.Rows)
                {
                    l_sli_Proveedores.Add(new SelectListItem { Value = fila["proveedor_id"].ToString(), Text = fila["proveedor_nombre"].ToString() });
                }
                ViewBag.Proveedores = l_sli_Proveedores;

                //DropDownList de monedas
                MonedaBL l_bl_Moneda = new MonedaBL();
                DataTable l_dt_Monedas = l_bl_Moneda.ConsultarMonedasActivas(0);// (model.MonedaId);
                List<SelectListItem> l_sli_Monedas = new List<SelectListItem>();
                foreach (DataRow fila in l_dt_Monedas.Rows)
                {
                    //if (fila["moneda_id"].ToString() == model.MonedaId.ToString())
                    //{
                    //    l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString(), Selected = true });
                    //}
                    //else
                    //{
                    l_sli_Monedas.Add(new SelectListItem { Value = fila["moneda_id"].ToString(), Text = fila["moneda_cod"].ToString() });
                    //}
                }
                ViewBag.Monedas = l_sli_Monedas;

                return PartialView();

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "UploadCatalogo [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Catálogo";
                return View("Error");
            }
            finally { }
        }

        [HttpPost]
        public ActionResult UploadCatalogo(FormCollection formCollection)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ArticuloController.cs", "UploadCatalogo [HttpPost]");

                CatalogoModel model = new CatalogoModel();
                string sArchivo = Session["UploadedFile"].ToString();
                string sUsuario = Session["UsuarioLogueado"].ToString();
                model.LoginCreacion = sUsuario;
                model.CBM = (Request.Form["ColCBM"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColCBM"].ToString());
                model.Codigo = (Request.Form["ColCodigo"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColCodigo"].ToString());
                model.CodigoBarras = (Request.Form["ColCodBarras"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColCodBarras"].ToString()); 
                model.Cotizacion = (Request.Form["ColCotizacion"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColCotizacion"].ToString());
                model.Descripcion = (Request.Form["ColDesc"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColDesc"].ToString());
                model.Descripcion2 = (Request.Form["ColDesc2"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColDesc2"].ToString());
                model.INNER = (Request.Form["ColINNER"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColINNER"].ToString());
                model.Moneda = (Request.Form["ColMoneda"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColMoneda"].ToString());
                model.Nombre = (Request.Form["ColNombre"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColNombre"].ToString());
                model.Observaciones1 = (Request.Form["ColObs1"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColObs1"].ToString());
                model.Observaciones2 = (Request.Form["ColObs2"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColObs2"].ToString());
                model.Observaciones3 = (Request.Form["ColObs3"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColObs3"].ToString());
                model.PesoBruto = (Request.Form["ColPesoBruto"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColPesoBruto"].ToString());
                model.PesoNeto = (Request.Form["ColPesoNeto"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColPesoNeto"].ToString());
                model.Precio = (Request.Form["ColPrecio"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColPrecio"].ToString());
                model.ProveedorId = (Request.Form["ddlProveedor"].ToString() == "") ? (Int32)0 : Convert.ToInt32(Request.Form["ddlProveedor"].ToString());
                model.MonedaId = (Request.Form["ddlMoneda"].ToString() == "") ? (Int32)0 : Convert.ToInt32(Request.Form["ddlMoneda"].ToString());
                model.RMB = (Request.Form["ColRMB"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColRMB"].ToString());
                model.Rubro = (Request.Form["ColRubro"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColRubro"].ToString());
                model.UniXbulto = (Request.Form["ColUnXBulto"].ToString() == "") ? (Int16)0 : Convert.ToInt16(Request.Form["ColUnXBulto"].ToString());

                //A todos les resto 1 porque el usuario ingresa desde la columna 1, pero VS subindica desde el 0.
                model.CBM -= 1;
                model.Codigo -= 1;
                model.CodigoBarras -= 1;
                model.Cotizacion -= 1;
                model.Descripcion -= 1;
                model.Descripcion2 -= 1;
                model.INNER -= 1;
                model.Moneda -= 1;
                model.Nombre -= 1;
                model.Observaciones1 -= 1;
                model.Observaciones2 -= 1;
                model.Observaciones3 -= 1;
                model.PesoBruto -= 1;
                model.PesoNeto -= 1;
                model.Precio -= 1;
                model.RMB -= 1;
                model.Rubro -= 1;
                model.UniXbulto -= 1;
         
                if (ModelState.IsValid)
                {
                    ArticuloBL l_bl_Articulo = new ArticuloBL();
                    l_s_Mensaje = l_bl_Articulo.CargarArticulos(model, sArchivo);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        ViewBag.ErrorObject = "Catálogo";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = l_s_Mensaje;
                    ViewBag.ErrorObject = "Catálogo";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ArticuloController.cs", "UploadCatalogo [HttpPost]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Catálogo";
                return View("Error");
            }
            finally { }
        }
    }
}