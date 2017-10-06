using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Modelos;
using SisCompras.BL;

namespace SisComprasWebApp.Controllers
{
    public class ProveedorController : Controller
    {

        // GET: Proveedor
        [HttpGet]
        public ActionResult Index()
        {

            if (SesionExpirada())
            {
                return View("ErrorSession");
            }

            ProveedorBL l_bl_Proveedor = new ProveedorBL();
            DataTable l_dt_Proveedores = new DataTable();

            l_dt_Proveedores = l_bl_Proveedor.ConsultarProveedores();

            return View(l_dt_Proveedores);
        }

        //El video https://www.youtube.com/watch?v=1IFS33sPDhE dice que no usaremos esto:
        //// GET: Proveedor/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Proveedor/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ProveedorModel();
            model.domicilio = new DomicilioModel();//¿Esto está bien manejado así?

            model.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //DropDownList de paises
            PaisBL l_bl_Pais = new PaisBL();
            DataTable l_dt_Paises = l_bl_Pais.ConsultarPaisesActivos(0);
            List<SelectListItem> l_sli_Paises = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Paises.Rows)
            {
                l_sli_Paises.Add(new SelectListItem { Value = fila["pais_id"].ToString(), Text = fila["pais_nombre"].ToString() });
            }
            model.domicilio.PaisesActivos = l_sli_Paises;

            //DropDownList de provincias
            ProvinciaBL l_bl_Provincia = new ProvinciaBL();
            DataTable l_dt_Provincias = l_bl_Provincia.ConsultarProvinciasActivas(0);
            List<SelectListItem> l_sli_Provincias = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Provincias.Rows)
            {
                l_sli_Provincias.Add(new SelectListItem { Value = fila["provincia_id"].ToString(), Text = fila["provincia_nombre"].ToString() });
            }
            model.domicilio.ProvinciasActivas = l_sli_Provincias;

            //DropDownList de localidades
            LocalidadBL l_bl_Localidad = new LocalidadBL();
            DataTable l_dt_Localidades = l_bl_Localidad.ConsultarLocalidadesActivas(0);
            List<SelectListItem> l_sli_Localidades = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Localidades.Rows)
            {
                l_sli_Localidades.Add(new SelectListItem { Value = fila["localidad_id"].ToString(), Text = fila["localidad_nombre"].ToString() });
            }
            model.domicilio.LocalidadesActivas = l_sli_Localidades;

            //DropDownList de tipos de domicilio
            GeneralesBL l_bl_Generales = new GeneralesBL();
            DataTable l_dt_Valores = l_bl_Generales.ConsultarConjuntoValores("TIPOS_DE_DOMICILIO", "", "", "");
            List<SelectListItem> l_sli_TiposDomi = new List<SelectListItem>();
            foreach (DataRow fila in l_dt_Valores.Rows)
            {
                if (fila["flag_default"].ToString() == "Si")
                {
                    l_sli_TiposDomi.Add(new SelectListItem { Value = fila["valor_codigo"].ToString(), Text = fila["valor_desc"].ToString(), Selected = true });
                }
                else
                {
                    l_sli_TiposDomi.Add(new SelectListItem { Value = fila["valor_codigo"].ToString(), Text = fila["valor_desc"].ToString() });
                }
            }
            model.domicilio.TiposDomicilio = l_sli_TiposDomi;

            return View(model);
        }

        // POST: Proveedor/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Create(ProveedorModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando", "ProveedorController.cs", "Crete [HttpPost]");

                //Puta madre!! no me anda nada de esto
                //http://techfunda.com/howto/259/passing-error-to-view-from-controller-action

                if (ModelState.IsValid)
                {
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginCreacion = sUsuario;

                    ProveedorBL l_bl_Proveedor = new ProveedorBL();
                    l_s_Mensaje = l_bl_Proveedor.Insertar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID del proveedor generado
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = l_s_Mensaje;
                            ViewBag.ErrorObject = "Proveedor";
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
                    ViewBag.ErrorObject = "Proveedor";
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorController.cs", "Create [HttpPost]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Proveedor";
                return View("Error");
            }
            finally { }
        }

        // GET: Proveedor/Edit/5
        public ActionResult Edit(int id)
        {
            string l_s_Mensaje = "";
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + id.ToString(), "ProveedorController.cs", "Edit [HttpGet]");

                ProveedorModel model = new ProveedorModel();
                ProveedorBL l_bl_Proveedor = new ProveedorBL();

                if(model==null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    model = l_bl_Proveedor.Consultar(id);
                    model.Activos = new List<SelectListItem> {
                    new SelectListItem { Value = "Si", Text = "Si"},
                    new SelectListItem { Value = "No", Text = "No"}
                    };

                    //DropDownList de paises
                    PaisBL l_bl_Pais = new PaisBL();
                    DataTable l_dt_Paises = l_bl_Pais.ConsultarPaisesActivos(0);
                    List<SelectListItem> l_sli_Paises = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Paises.Rows)
                    {
                        l_sli_Paises.Add(new SelectListItem { Value = fila["pais_id"].ToString(), Text = fila["pais_nombre"].ToString() });
                    }
                    model.domicilio.PaisesActivos = l_sli_Paises;

                    //DropDownList de provincias
                    ProvinciaBL l_bl_Provincia = new ProvinciaBL();
                    DataTable l_dt_Provincias = l_bl_Provincia.ConsultarProvinciasActivas(0);
                    List<SelectListItem> l_sli_Provincias = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Provincias.Rows)
                    {
                        l_sli_Provincias.Add(new SelectListItem { Value = fila["provincia_id"].ToString(), Text = fila["provincia_nombre"].ToString() });
                    }
                    model.domicilio.ProvinciasActivas = l_sli_Provincias;

                    //DropDownList de localidades
                    LocalidadBL l_bl_Localidad = new LocalidadBL();
                    DataTable l_dt_Localidades = l_bl_Localidad.ConsultarLocalidadesActivas(0);
                    List<SelectListItem> l_sli_Localidades = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Localidades.Rows)
                    {
                        l_sli_Localidades.Add(new SelectListItem { Value = fila["localidad_id"].ToString(), Text = fila["localidad_nombre"].ToString() });
                    }
                    model.domicilio.LocalidadesActivas = l_sli_Localidades;

                    //DropDownList de tipos de domicilio
                    GeneralesBL l_bl_Generales = new GeneralesBL();
                    DataTable l_dt_Valores = l_bl_Generales.ConsultarConjuntoValores("TIPOS_DE_DOMICILIO", "", "", "");
                    List <SelectListItem> l_sli_TiposDomi = new List<SelectListItem>();
                    foreach (DataRow fila in l_dt_Valores.Rows)
                    {
                        if(fila["flag_default"].ToString() == "Si")
                        {
                            l_sli_TiposDomi.Add(new SelectListItem { Value = fila["valor_codigo"].ToString(), Text = fila["valor_desc"].ToString(), Selected=true});
                        }
                        else
                        {
                            l_sli_TiposDomi.Add(new SelectListItem { Value = fila["valor_codigo"].ToString(), Text = fila["valor_desc"].ToString() });
                        }
                    }
                    model.domicilio.TiposDomicilio = l_sli_TiposDomi;

                    return View(model);
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorController.cs", "Edit [HttpGet]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Proveedor";
                return View("Error");

            }
            finally { }
        }

        // POST: Proveedor/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Edit(ProveedorModel model)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID.ToString(), "ProveedorController.cs", "Edit [HttpPost]");

                if (ModelState.IsValid)
                {
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginUltModif = sUsuario;

                    ProveedorBL l_bl_Proveedor = new ProveedorBL();
                    l_s_Mensaje = l_bl_Proveedor.Actualizar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        ViewBag.ErrorObject = "Proveedor";
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
                    ViewBag.ErrorObject = "Proveedor";
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorController.cs", "Edit");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Proveedor";
                return View("Error");
            }
            finally { }
        }

        // GET: Proveedor/Delete/5
        public ActionResult Delete(int id)
        {

            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpGet): " + id.ToString(), "ProveedorController.cs", "Delete");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                ProveedorBL l_bl_Proveedor = new ProveedorBL();
                l_s_Mensaje = l_bl_Proveedor.Eliminar(id, sUsuario);

                if (l_s_Mensaje == "")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = l_s_Mensaje;
                    ViewBag.ErrorObject = "Proveedor";
                    //ModelState.AddModelError("", "Please write first name.");
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProveedorController.cs", "Delete");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Proveedor";
                return View("Error");
            }
            finally { }
        }

        //// POST: Proveedor/Delete/5 ==> en el video esta parte la borró
        //                                https://www.youtube.com/watch?v=1IFS33sPDhE
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

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CreateForValidation(ProveedorModel model)
        {
            Int32 l_i_Resultado = 0;

            //http://techfunda.com/howto/259/passing-error-to-view-from-controller-action

            if (ModelState.IsValid)
            {
                if (model.Codigo == null || model.Codigo == "")
                {
                    ModelState.AddModelError("Codigo", "El código es obligatorio.");
                }

                if (model.Nombre == null || model.Nombre == "")
                {
                    ModelState.AddModelError("Nombre", "El nombre es obligatorio.");
                }

                if (Int32.TryParse(model.Codigo, out l_i_Resultado) == false)
                {
                    ModelState.AddModelError("Codigo", "El código acepta sólo números.");
                }

            }

            return View(model);
        }

        private bool SesionExpirada()
        {
            try
            {
                string sUsuario = Session["UsuarioLogueado"].ToString();
                if (sUsuario == "")  return true;
                else return false;
            }
            catch (Exception miEx)
            {
                return true;
            }

        }

    }
}
