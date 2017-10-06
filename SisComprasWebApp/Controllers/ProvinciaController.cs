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
    public class ProvinciaController : Controller
    {

        // GET: Provincia
        [HttpGet]
        public ActionResult Index()
        {
            if (SesionExpirada())
            {
                return View("ErrorSession");
            }

            ProvinciaBL l_bl_Provincia = new ProvinciaBL();
            DataTable l_dt_Provincias = new DataTable();

            l_dt_Provincias = l_bl_Provincia.ConsultarProvincias();

            return View(l_dt_Provincias);
        }

        //El video https://www.youtube.com/watch?v=1IFS33sPDhE dice que no usaremos esto:
        //// GET: Provincia/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Provincia/Create
        [HttpGet]
        public ActionResult Create()
        {
            var l_model_Provincia = new ProvinciaModel();
            l_model_Provincia.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //return View(new ProvinciaModel());
            return View(l_model_Provincia);
        }

        // POST: Provincia/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Create(ProvinciaModel p_model_Provincia)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpPost)", "ProvinciaController.cs", "Crete");

                if (ModelState.IsValid)
                {
                    ProvinciaBL l_bl_Provincia = new ProvinciaBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    p_model_Provincia.LoginCreacion = sUsuario;
                    l_s_Mensaje = l_bl_Provincia.Insertar(p_model_Provincia);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID de la provincia generado
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = l_s_Mensaje;
                            ViewBag.ErrorObject = "Provincia";
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
                    ViewBag.ErrorObject = "Provincia";
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaController.cs", "Create");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Provincia";
                //ModelState.AddModelError("", "Please write first name.");
                return View("Error");
            }
        }

        // GET: Provincia/Edit/5
        public ActionResult Edit(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                ProvinciaModel l_model_Provincia = new ProvinciaModel();
                ProvinciaBL l_bl_Provincia = new ProvinciaBL();

                if (l_model_Provincia == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    l_model_Provincia = l_bl_Provincia.Consultar(id);
                    l_model_Provincia.Activos = new List<SelectListItem> {
                    new SelectListItem { Value = "Si", Text = "Si"},
                    new SelectListItem { Value = "No", Text = "No"}
                    };
                    return View(l_model_Provincia);
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaController.cs", "Edit");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Provincia";
                //ModelState.AddModelError("", "Please write first name.");
                return View("Error");
            }
            finally { }
        }

        // POST: Provincia/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Edit(ProvinciaModel p_model_Provincia)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                if (ModelState.IsValid)
                {
                    ProvinciaBL l_bl_Provincia = new ProvinciaBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    p_model_Provincia.LoginUltModif = sUsuario;
                    l_s_Mensaje = l_bl_Provincia.Actualizar(p_model_Provincia);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        ViewBag.ErrorObject = "Provincia";
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
                    ViewBag.ErrorObject = "Provincia";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaController.cs", "Edit");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Provincia";
                //ModelState.AddModelError("", "Please write first name.");
                return View("Error");
            }
            finally { }
        }

        // GET: Provincia/Delete/5
        public ActionResult Delete(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpGet): " + id.ToString(), "ProvinciaController.cs", "Delete");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                ProvinciaBL l_bl_Provincia = new ProvinciaBL();
                l_s_Mensaje = l_bl_Provincia.Eliminar(id, sUsuario);

                return RedirectToAction("Index");
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "ProvinciaController.cs", "Delete");
                ViewBag.ErrorMessage = l_s_Mensaje;
                ViewBag.ErrorObject = "Provincia";
                //ModelState.AddModelError("", "Please write first name.");
                return View("Error");
            }
            finally { }
        }

        // POST: Provincia/Delete/5 ==> en el video esta parte la borró
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
