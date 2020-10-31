using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestauranteWeb.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador
        public ActionResult Inicio()
        {
            return View();
        }
        public ActionResult Roles()
        {
            return View();
        }

        public ActionResult EditarCuenta()
        {
            return View();
        }

        public ActionResult ActualizarContraseña()
        {
            return View();
        }

        public ActionResult Empleados()
        {
            return View();
        }
    }
}