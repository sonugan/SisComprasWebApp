using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelos;
using System.Data;
using SisCompras.BL;

namespace SisComprasWebApp.Controllers
{
    public class PaisController : Controller
    {

        // GET: Pais
        [HttpGet]
        public ActionResult Index()
        {
            PaisBL l_bl_Pais = new PaisBL();
            DataTable l_dt_Paises = new DataTable();

            l_dt_Paises = l_bl_Pais.ConsultarPaises();

            return View(l_dt_Paises);
        }

        //El video https://www.youtube.com/watch?v=1IFS33sPDhE dice que no usaremos esto:
        //// GET: Pais/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Pais/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new PaisModel();
            model.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //return View(new PaisModel());
            return View(model);
        }

        // POST: Pais/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Create(PaisModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpPost)", "PaisController.cs", "Crete");

                if (ModelState.IsValid)
                {
                    PaisBL l_bl_Pais = new PaisBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginCreacion = sUsuario;
                    l_s_Mensaje = l_bl_Pais.Insertar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID del país generado
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = l_s_Mensaje;
                            ViewBag.ErrorObject = "País";
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
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisController.cs", "Create");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "País";
                return View("Error");
            }
        }

        // GET: Pais/Edit/5
        public ActionResult Edit(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + id.ToString(), "PaisController.cs", "Edit [HttpGet]");

                PaisModel model = new PaisModel();
                PaisBL l_bl_Pais = new PaisBL();

                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    model = l_bl_Pais.Consultar(id);
                    model.Activos = new List<SelectListItem> {
                    new SelectListItem { Value = "Si", Text = "Si"},
                    new SelectListItem { Value = "No", Text = "No"}
                    };
                    return View(model);
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisController.cs", "Edit [HttpGet]");
                //return View();
                return RedirectToAction("Index");
            }
            finally { }
        }

        // POST: Pais/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Edit(PaisModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando: " + model.ID.ToString(), "PaisController.cs", "Edit [HttpPost]");

                if (ModelState.IsValid)
                {
                    PaisBL l_bl_Pais = new PaisBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginUltModif = sUsuario;
                    l_s_Mensaje = l_bl_Pais.Actualizar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        //ModelState.AddModelError("", "Please write first name.");
                        ViewBag.ErrorObject = "País";
                        return View("Error");
                    }

                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    ViewBag.ErrorMessage = "Los datos ingresados no son válidos: " + message.ToString();
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisController.cs", "Edit [HttpPost]");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "País";
                return View("Error");
            }
            finally { }
        }

        // GET: Pais/Delete/5
        public ActionResult Delete(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpGet): " + id.ToString(), "PaisController.cs", "Delete");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                PaisBL l_bl_Pais = new PaisBL();
                l_s_Mensaje = l_bl_Pais.Eliminar(id, sUsuario);

                if (l_s_Mensaje == "")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = l_s_Mensaje;
                    //ModelState.AddModelError("", "Please write first name.");
                    ViewBag.ErrorObject = "País";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "PaisController.cs", "Delete");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "País";
                return View("Error");
            }
            finally { }
        }

        // POST: Pais/Delete/5 ==> en el video esta parte la borró
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
        public ActionResult CreateForValidation(PaisModel model)
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
    }
}
