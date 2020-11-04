using Newtonsoft.Json;
using RestauranteWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RestauranteWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private ProyectoASP_RestauranteEntities db = new ProyectoASP_RestauranteEntities();
        // GET: Administrador
        public ActionResult Inicio()
        {
            return View();
        }

        // INICIO METODOS ROLES
        [HttpGet]
        public ActionResult Roles()
        {
            return View(db.RolesEmpleados.ToList());
        }

        public List<RolesEmpleados>  recargarRoles()
        {
            return db.RolesEmpleados.ToList();
        }


        [HttpPost]
        public void Roles([Bind(Include = "IdRol,Rol")] RolesEmpleados rolesEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.RolesEmpleados.Add(rolesEmpleados);
                db.SaveChanges();
                //return RedirectToAction("Roles");
            }
           // return View();
        }

        [HttpPost]
        public ActionResult editarRol([Bind(Include = "IdRol,Rol")] RolesEmpleados rolesEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolesEmpleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Roles");
            }
            return View();
        }

        [HttpPost]
        public JsonResult eliminarRol(int? idRol)
        {
            try
            {
                RolesEmpleados rolesEmpleados = db.RolesEmpleados.Find(idRol);
                db.RolesEmpleados.Remove(rolesEmpleados);
                db.SaveChanges();

                return Json(new {success = true});

            }catch( DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }
        [HttpPost]
        public JsonResult reloadRoles()
        {
            List<RolesEmpleados> roles = new List<RolesEmpleados>();
            roles = db.RolesEmpleados.ToList();
            var rolesClean = roles.Select(s=> new {
                IdRol = s.IdRol,
                Rol = s.Rol
            });
            return Json(rolesClean, JsonRequestBehavior.AllowGet);
        }
        // FIN METODOS ROLES

        public ActionResult EditarCuenta()
        {
            return View();
        }

        public ActionResult ActualizarContraseña()
        {
            return View();
        }
        //INICIO METODOS CUENTAS EMPLEADOS
        [HttpGet]
        public ActionResult Empleados()
        {
            ViewBag.IdRoles = new SelectList(db.RolesEmpleados, "IdRol", "Rol");
            var cuentasEmpleados = db.CuentasEmpleados.Include(c => c.RolesEmpleados);
            return View(cuentasEmpleados.ToList());
        }

        [HttpPost]
        public void Empleados([Bind(Include = "Usuario,IdRol,Nombres,Apellidos,Clave")] CuentasEmpleados cuentasEmpleados)
        {
            if (ModelState.IsValid)
            {
                cuentasEmpleados.Clave = Crypto.Hash(cuentasEmpleados.Clave);

                db.CuentasEmpleados.Add(cuentasEmpleados);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void editarClave(string usuario, string clave)
        {
            CuentasEmpleados cuentasEmpleados = db.CuentasEmpleados.Find(usuario);
            cuentasEmpleados.Clave = Crypto.Hash(clave);
            db.Entry(cuentasEmpleados).State = EntityState.Modified;
            db.SaveChanges();
        }

        [HttpPost]
        public void editarEmpleados([Bind(Include = "Usuario,IdRol,Nombres,Apellidos")] CuentasEmpleados cuentasEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuentasEmpleados).State = EntityState.Modified;
                db.SaveChanges();              
            }
        }

        [HttpPost]
        public JsonResult eliminarEmpleado(string usuario)
        {
            try
            {
                CuentasEmpleados cuentasEmpleados = db.CuentasEmpleados.Find(usuario);
                db.CuentasEmpleados.Remove(cuentasEmpleados);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch(DbUpdateException ex)
            { return Json(new { success = false, message=ex.Message}); }
        }

        [HttpPost]
        public JsonResult reloadEmpleados()
        {
            List<CuentasEmpleados> empleados = new List<CuentasEmpleados>();
            empleados = db.CuentasEmpleados.Include(c=>c.RolesEmpleados).ToList();
            var empleadosClean = empleados.Select(s=> new {
                s.Usuario,
                s.Nombres,
                s.Apellidos,
                s.RolesEmpleados.Rol,
                s.RolesEmpleados.IdRol
            });
            return Json(empleadosClean, JsonRequestBehavior.AllowGet);
        }
        //FIN METODOS CUENTAS EMPLEADOS
        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult Categorias()
        {
            return View();
        }
       
        public ActionResult Estados()
        {
            return View();
        }
    }
}