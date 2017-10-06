using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisCompras.BL;
using Modelos;

namespace SisComprasWebApp.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioBusinessLogic ublUsuario = new UsuarioBusinessLogic();

        // GET: Usuario
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsuarioModel pUsuario)
        {
            string sMensaje = "";

            if (ModelState.IsValid)
            {
                sMensaje = ublUsuario.ValidarUsuarioIngreso(pUsuario);
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                sMensaje = "Error: " + message.ToString();
            }

            Session["UsuarioLogueado"] = "";

            if (Request.IsAjaxRequest())
            {
                if (sMensaje == "") //usuario logueado OK
                {
                    Session["UsuarioLogueado"] = pUsuario.Usuario.ToUpper();
                }
                return Json(sMensaje, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}