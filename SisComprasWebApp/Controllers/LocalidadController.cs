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
    public class LocalidadController : Controller
    {

        // GET: Localidad
        [HttpGet]
        public ActionResult Index()
        {
            if (SesionExpirada())
            {
                return View("ErrorSession");
            }

            LocalidadBL l_bl_Localidad = new LocalidadBL();
            DataTable l_dt_Localidades = new DataTable();

            l_dt_Localidades = l_bl_Localidad.ConsultarLocalidades();

            return View(l_dt_Localidades);
        }

        //El video https://www.youtube.com/watch?v=1IFS33sPDhE dice que no usaremos esto:
        //// GET: Localidad/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Localidad/Create
        [HttpGet]
        public ActionResult Create()
        {
            var l_model_Localidad = new LocalidadModel();
            l_model_Localidad.Activos = new List<SelectListItem> {
            new SelectListItem { Value = "Si", Text = "Si"},
            new SelectListItem { Value = "No", Text = "No"}
            };

            //return View(new LocalidadModel());
            return View(l_model_Localidad);
        }

        // POST: Localidad/Create ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Create(LocalidadModel p_model_Localidad)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";
            int l_i_Resultado = 0;

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpPost)", "LocalidadController.cs", "Crete");

                if (ModelState.IsValid)
                {
                    LocalidadBL l_bl_Localidad = new LocalidadBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    p_model_Localidad.LoginCreacion = sUsuario;
                    l_s_Mensaje = l_bl_Localidad.Insertar(p_model_Localidad);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Int32.TryParse(l_s_Mensaje, out l_i_Resultado))//Es el ID de la localidad generado
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = l_s_Mensaje;
                            ViewBag.ErrorObject = "Localidad";
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
                    ViewBag.ErrorObject = "Localidad";
                    return View("Error");
                }
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadController.cs", "Create");
                //return View();
                return RedirectToAction("Index");
            }
        }

        // GET: Localidad/Edit/5
        public ActionResult Edit(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                LocalidadModel l_model_Localidad = new LocalidadModel();
                LocalidadBL l_bl_Localidad = new LocalidadBL();

                if (l_model_Localidad == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    l_model_Localidad = l_bl_Localidad.Consultar(id);
                    l_model_Localidad.Activos = new List<SelectListItem> {
                    new SelectListItem { Value = "Si", Text = "Si"},
                    new SelectListItem { Value = "No", Text = "No"}
                    };
                    return View(l_model_Localidad);
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadController.cs", "Edit");
                //return View();
                return RedirectToAction("Index");
            }
            finally { }
        }

        // POST: Localidad/Edit/5 ==> esto fue creado por VS. Lo cambié por el de abajo según el video
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
        public ActionResult Edit(LocalidadModel p_model_Localidad)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {

                if (ModelState.IsValid)
                {
                    LocalidadBL l_bl_Localidad = new LocalidadBL();
                    string sUsuario = Session["UsuarioLogueado"].ToString();
                    p_model_Localidad.LoginUltModif = sUsuario;
                    l_s_Mensaje = l_bl_Localidad.Actualizar(p_model_Localidad);

                    if (l_s_Mensaje == "")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = l_s_Mensaje;
                        ViewBag.ErrorObject = "Localidad";
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
                    ViewBag.ErrorObject = "Localidad";
                    return View("Error");
                }

            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadController.cs", "Edit");
                return View();
            }
            finally { }
        }

        // GET: Localidad/Delete/5
        public ActionResult Delete(int id)
        {
            AplicacionLog.Logueo l_log_Objeto = new AplicacionLog.Logueo();
            string l_s_Mensaje = "";

            try
            {
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_DEBUG, "Ingresando (HttpGet): " + id.ToString(), "LocalidadController.cs", "Delete");

                string sUsuario = Session["UsuarioLogueado"].ToString();
                LocalidadBL l_bl_Localidad = new LocalidadBL();
                l_s_Mensaje = l_bl_Localidad.Eliminar(id, sUsuario);

                return RedirectToAction("Index");
            }
            catch (Exception miEx)
            {
                l_s_Mensaje = miEx.Message.ToString();
                System.Diagnostics.Debug.WriteLine(l_s_Mensaje);
                l_log_Objeto.RegistraEnArchivoLog(AplicacionLog.Logueo.LOGL_ERROR, l_s_Mensaje, "LocalidadController.cs", "Delete");
                return View();
            }
            finally { }
        }

        // POST: Localidad/Delete/5 ==> en el video esta parte la borró
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
