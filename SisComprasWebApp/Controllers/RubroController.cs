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
    public class RubroController : Controller
    {
        // GET: Rubro
        [HttpGet]
        public ActionResult Index()
        {

            if (SesionExpirada())
            {
                return View("ErrorSession");
            }

            RubroBL l_bl_Rubro = new RubroBL();
            DataTable l_dt_Rubros = new DataTable();

            l_dt_Rubros = l_bl_Rubro.ConsultarRubros();

            return View(l_dt_Rubros);
        }

        //El video https://www.youtube.com/watch?v=1IFS33sPDhE dice que no usaremos esto:
        //// GET: Rubro/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Rubro/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new RubroModel();
            model.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //return View(new RubroModel());
            return View(model);
        }

        // POST: Rubro/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Create(RubroModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpPost)", "RubroController.cs", "Crete");

                if (ModelState.IsValid)
                {
                    RubroBL l_bl_Rubro = new RubroBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginCreacion = sUsuario;
                    l_s_Mensaje = l_bl_Rubro.Insertar(model);

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
                            //ModelState.AddModelError("", "Please write first name.");
                            ViewBag.ErrorObject = "Rubro";
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroController.cs", "Create");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Rubro";
                return View("Error");
            }
        }

        // GET: Rubro/Edit/5
        public ActionResult Edit(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                RubroModel model = new RubroModel();
                RubroBL l_bl_Rubro = new RubroBL();

                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    model = l_bl_Rubro.Consultar(id);
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroController.cs", "Edit");
                //return View();
                return RedirectToAction("Index");
            }
            finally { }
        }

        // POST: Rubro/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Edit(RubroModel model)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                if (ModelState.IsValid)
                {
                    RubroBL l_bl_Rubro = new RubroBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    model.LoginUltModif = sUsuario;
                    l_s_Mensaje = l_bl_Rubro.Actualizar(model);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        //ModelState.AddModelError("", "Please write first name.");
                        ViewBag.ErrorObject = "Rubro";
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
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroController.cs", "Edit");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Rubro";
                return View("Error");
            }
            finally { }
        }

        // GET: Rubro/Delete/5
        public ActionResult Delete(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpGet): " + id.ToString(), "RubroController.cs", "Delete");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                RubroBL l_bl_Rubro = new RubroBL();
                l_s_Mensaje = l_bl_Rubro.Eliminar(id, sUsuario);

                if (l_s_Mensaje == "")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = l_s_Mensaje;
                    //ModelState.AddModelError("", "Please write first name.");
                    ViewBag.ErrorObject = "Rubro";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "RubroController.cs", "Delete");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Rubro";
                return View("Error");
            }
            finally { }
        }

        // POST: Rubro/Delete/5 ==> en el video esta parte la borró
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
        public ActionResult CreateForValidation(RubroModel model)
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
                if (sUsuario == "") return true;
                else return false;
            }
            catch (Exception miEx)
            {
                return true;
            }

        }

    }
}
