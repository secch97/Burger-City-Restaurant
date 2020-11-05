using Newtonsoft.Json;
using RestauranteWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
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

        //INICIO METODOS CUENTAS CLIENTES
        public ActionResult Clientes()
        {
            return View(db.CuentasClientes.ToList());
        }

        [HttpPost]
        public JsonResult obtenerCliente(string idCliente)
        {
            CuentasClientes cuentasClientes = db.CuentasClientes.Find(idCliente);
            
            return Json(new { nombres = cuentasClientes.Nombres,
                              apellidos= cuentasClientes.Apellidos,
                              telefono = cuentasClientes.TelefonoFijo,
                              movil = cuentasClientes.TelefonoCelular,
                              correo = cuentasClientes.Correo,
                              direccionCliente = cuentasClientes.Direccion,
                              direccionEntrega= cuentasClientes.DireccionEntrega});
        }

        [HttpPost]
        public JsonResult eliminarCliente(string idCliente)
        {
            try
            {
                CuentasClientes cuentasClientes = db.CuentasClientes.Find(idCliente);
                db.CuentasClientes.Remove(cuentasClientes);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadClientes()
        {
            List<CuentasClientes> clientes = new List<CuentasClientes>();
            clientes = db.CuentasClientes.ToList();
            var clientesClean = clientes.Select(s => new {
                s.IdCliente,
                s.Nombres,
                s.Apellidos,
                s.Fecha
            });
            return Json(clientesClean, JsonRequestBehavior.AllowGet);
        }

        //FIN METODOS CUENTAS CLIENTES

        //INCIO METODOS ESTADOS
        public ActionResult Estados()
        {
            return View(db.EstadosProductos.ToList());
        }

        [HttpPost]
        public void Estados([Bind(Include = "IdEstado,Nombre")] EstadosProductos estadosProductos)
        {
            if (ModelState.IsValid)
            {
                db.EstadosProductos.Add(estadosProductos);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void editarEstados([Bind(Include = "IdEstado,Nombre")] EstadosProductos estadosProductos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadosProductos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public JsonResult eliminarEstado(int idEstado)
        {
            try
            {
                EstadosProductos estadosProductos = db.EstadosProductos.Find(idEstado);
                db.EstadosProductos.Remove(estadosProductos);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadEstados()
        {
            List<EstadosProductos> estados= new List<EstadosProductos>();
            estados = db.EstadosProductos.ToList();
            var clientesClean = estados.Select(s => new {
                s.IdEstado,
                s.Nombre
            });
            return Json(clientesClean, JsonRequestBehavior.AllowGet);
        }


        //FIN METODOS ESTADOS

        //INICIO METODOS CATEGORIAS
        public ActionResult Categorias()
        {
            List<CategoriasProductos> list;
            list = db.CategoriasProductos.ToList();
            return View(db.CategoriasProductos.ToList());
        }

        [HttpPost]
        public JsonResult Categorias( HttpPostedFileBase imagenCategoriaRegistro, string nombreCategoria, string descripcionCategoria)
        {
            string rutaBase = "/Content/imagenes/";
            string carpeta = "Categorias/";
            string ruta = "";
            
            
            if (ModelState.IsValid)
            {
                //Verificando que archivo no exista 
                ruta = obtenerRuta(imagenCategoriaRegistro, rutaBase, carpeta, -1);

                if (operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre "+obtenerRuta(imagenCategoriaRegistro,"","",0)+" ya se encuentra registrado" });
                }
                //Guardando registro de categoria
                CategoriasProductos categoriasProductos = new CategoriasProductos();
                categoriasProductos.IdCategoria = db.GeneradorIdOjetos("CAT");
                categoriasProductos.Nombre = nombreCategoria;
                categoriasProductos.Descripcion = descripcionCategoria;
                categoriasProductos.Imagen = obtenerRuta(imagenCategoriaRegistro, rutaBase, carpeta, 1);
                db.CategoriasProductos.Add(categoriasProductos);
                db.SaveChanges();

                //Guardando imagen
                imagenCategoriaRegistro.SaveAs(ruta);
                
                //respuesta
                return Json(new { success = true, message = "Categoría registrada con éxito" });

            }

              return Json(new { success = false, message = "El modelo no fue válido. Contacte a soporte"});

        }

        [HttpPost]
        public JsonResult editarCategorias(HttpPostedFileBase editarNuevaimagenCategoria, string editarNombreCategoria, string editarDescripcionCategoria, string imagenActual, string idCategoria)
        {
            string rutaBase = "/Content/imagenes/";
            string carpeta = "Categorias/";
            string ruta = "";

            CategoriasProductos categoria = new CategoriasProductos();

            categoria = db.CategoriasProductos.Find(idCategoria);
            categoria.Nombre = editarNombreCategoria;
            categoria.Descripcion = editarDescripcionCategoria;

            if (editarNuevaimagenCategoria != null)
            {
                //Verificando que el nuevo archivo no exista
                ruta = obtenerRuta(editarNuevaimagenCategoria, rutaBase, carpeta, -1);

                if(operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre " + obtenerRuta(editarNuevaimagenCategoria, "", "", 0) + " ya se encuentra registrado" });
                }


                //eliminando imagen anterior
                operacionesImagenes(null, imagenActual, -1);

                //Asignando ruta nueva imagen
                categoria.Imagen = obtenerRuta(editarNuevaimagenCategoria, rutaBase, carpeta, 1);

                //almacenando nueva imagen
                operacionesImagenes(editarNuevaimagenCategoria, ruta, 1);
                
            }

            //Registrando cambios

            db.Entry(categoria).State = EntityState.Modified;
            db.SaveChanges();

            //Respuesta
            return Json(new { success = true, message = "Se actualizó con éxito" });

        }

        [HttpPost]
        public JsonResult eliminarCategoria(string idCategoria)
        {
            try
            {
                CategoriasProductos categorias = db.CategoriasProductos.Find(idCategoria);
                db.CategoriasProductos.Remove(categorias);
                db.SaveChanges();

                //eliminando imagen asociada
                operacionesImagenes(null, categorias.Imagen, -1);

                return Json(new { success = true, message = "Se eliminó con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadCategorias()
        {
            List<CategoriasProductos> categorias = new List<CategoriasProductos>();
            categorias = db.CategoriasProductos.ToList();
            var categoriasClean = categorias.Select(s => new {
                s.IdCategoria,
                s.Nombre,
                s.Imagen,
                s.Descripcion
            });
            return Json(categoriasClean, JsonRequestBehavior.AllowGet);
        }

        //FIN METODOS CATEGORIAS




        public ActionResult Productos()
        {
            return View();
        }

        public ActionResult Combos()
        {
            return View();
        }

        public ActionResult GestionarCombo()
        {
            return View();
        }

        public ActionResult Etapas()
        {
            return View();
        }

        public ActionResult HistorialDeOrdenes()
        {
            return View();
        }

        // INICIO METODOS AUXILIARES
        private bool operacionesImagenes(HttpPostedFileBase imagen, string ruta, int operacion)
        {/*las operaciones posibles
            -1 --> Eliminar
             0 --> verificar existencia
             1 --> Guardar*/
            bool respuesta = false;

            try
            {
                switch (operacion)
                {
                    case -1:
                        System.IO.File.Delete(Server.MapPath(ruta));
                        respuesta = true;
                        break;

                    case 0:
                        respuesta = System.IO.File.Exists(ruta);
                        break;

                    case 1:
                        imagen.SaveAs(ruta);
                        respuesta = true;
                        break;
                }
            }catch(Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }

        private String obtenerRuta(HttpPostedFileBase imagen, string rutaBase, string carpeta,int opcion)
        {
            var nombreImagen = Path.GetFileName(imagen.FileName);

            if (opcion == -1)
                return Path.Combine(Server.MapPath(rutaBase + carpeta), nombreImagen);
            else if (opcion == 0)
                return nombreImagen;
            else
                return rutaBase + carpeta + nombreImagen;
        }

        // FIN METODOS AUXILIARES
    }
}