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
using System.Web.Security;

namespace RestauranteWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private ProyectoASP_RestauranteEntities db = new ProyectoASP_RestauranteEntities();
        // GET: Administrador

        //INICIO METODOS INICIO
        public ActionResult Inicio()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return HistorialOrdenes();
            }
        }

        //FIN METODOS INICIO


        // INICIO METODOS ROLES
        [HttpGet]
        public ActionResult Roles()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return View(db.RolesEmpleados.ToList());
            }
        }

        public List<RolesEmpleados> recargarRoles()
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
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    db.Entry(rolesEmpleados).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Roles");
                }
                return View();
            }
        }

        [HttpPost]
        public JsonResult eliminarRol(int? idRol)
        {
            try
            {
                RolesEmpleados rolesEmpleados = db.RolesEmpleados.Find(idRol);
                db.RolesEmpleados.Remove(rolesEmpleados);
                db.SaveChanges();

                return Json(new { success = true });

            } catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }
        [HttpPost]
        public JsonResult reloadRoles()
        {
            List<RolesEmpleados> roles = new List<RolesEmpleados>();
            roles = db.RolesEmpleados.ToList();
            var rolesClean = roles.Select(s => new {
                IdRol = s.IdRol,
                Rol = s.Rol
            });
            return Json(rolesClean, JsonRequestBehavior.AllowGet);
        }
        // FIN METODOS ROLES

        public ActionResult EditarCuenta()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                string id = Session["id"].ToString();
                var emp = db.CuentasEmpleados.Where(a => a.Usuario == id).FirstOrDefault();
                ViewBag.nombre = emp.Nombres;
                ViewBag.apellido = emp.Apellidos;
                return View();
            }
        }

        [HttpPost]
        public JsonResult EditarCuenta(string usuario,string nombre,string apellido)
        {
            
            var emp = db.CuentasEmpleados.Where(a => a.Usuario == usuario).FirstOrDefault();
            emp.Nombres = nombre;
            emp.Apellidos = apellido;
            db.SaveChanges();
            return Json(1);
        }

        public ActionResult ActualizarContraseña()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult ActualizarContraseña(string usuario, string nueva, string antigua)
        {
            var emp = db.CuentasEmpleados.Where(a => a.Usuario == usuario).FirstOrDefault();
            if (string.Compare(Crypto.Hash(nueva), emp.Clave) == 0)
            {  
                return Json(2);
            }
            else
            {
                if (string.Compare(Crypto.Hash(antigua), emp.Clave)==0)
                {
                    emp.Clave = Crypto.Hash(nueva); ;
                    db.SaveChanges();
                    return Json(1);
                }
                else
                {
                    return Json(3);
                }
                
            }
        }
        //INICIO METODOS CUENTAS EMPLEADOS
        [HttpGet]
        public ActionResult Empleados()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                ViewBag.IdRoles = new SelectList(db.RolesEmpleados, "IdRol", "Rol");
                var cuentasEmpleados = db.CuentasEmpleados.Include(c => c.RolesEmpleados);
                return View(cuentasEmpleados.ToList());
            }
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

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["nombre"] = null;
            Session["id"] = null;
            Session["email"] = null;
            Session["user"] = null;
            return RedirectToAction("inicio", "Cliente");
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
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return View(db.CuentasClientes.ToList());
            }
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
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return View(db.EstadosProductos.ToList());
            }
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
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                List<CategoriasProductos> list;
                list = db.CategoriasProductos.ToList();
                return View(db.CategoriasProductos.ToList());
            }
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

        //INICIO METODOS PRODUCTOS
        public ActionResult Productos()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                ViewBag.IdEstados = new SelectList(db.EstadosProductos, "IdEstado", "Nombre");
                ViewBag.IdCategorias = new SelectList(db.CategoriasProductos, "IdCategoria", "Nombre");
                var productos = db.ProductosRestaurante.Include(c => c.EstadosProductos)
                                                       .Include(c => c.CategoriasProductos);

                return View(productos.ToList());
            }
        }

        [HttpPost]
        public JsonResult Productos(FormCollection formCollection)
        {

            string rutaBase = "/Content/imagenes/";
            string carpeta = "Productos/";
            string ruta = "";
            HttpPostedFileBase imagen = Request.Files["imagenProductoRegistro"];


            if (ModelState.IsValid)
            {
                //Verificando que archivo no exista 
                ruta = obtenerRuta(imagen, rutaBase, carpeta, -1);

                if (operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre " + obtenerRuta(imagen, "", "", 0) + " ya se encuentra registrado" });
                }
                //Guardando registro de Producto
                ProductosRestaurante productosRestaurante = new ProductosRestaurante();
                productosRestaurante.IdProducto = db.GeneradorIdOjetos("PRO");
                productosRestaurante.IdCategoria = formCollection["categoriaProductoRegistro"];
                productosRestaurante.Nombre = formCollection["nombreProducto"];
                productosRestaurante.Descripcion = formCollection["descripcionProducto"];
                productosRestaurante.Precio = Convert.ToDecimal(formCollection["precioProducto"]);
                productosRestaurante.IdEstado = Convert.ToInt32(formCollection["estadoProductoRegistro"]);
                productosRestaurante.Imagen = obtenerRuta(imagen, rutaBase, carpeta, 1);
                db.ProductosRestaurante.Add(productosRestaurante);
                db.SaveChanges();

                //Guardando imagen
                imagen.SaveAs(ruta);

                //respuesta
                return Json(new { success = true, message = "Producto registrado con éxito" });

            }

            return Json(new { success = false, message = "El modelo no fue válido. Contacte a soporte" });

        }

        [HttpPost]
        public JsonResult obtenerProducto(string idProducto)
        {
            ProductosRestaurante producto = db.ProductosRestaurante.Find(idProducto);

            return Json(new
            {
                nombreProducto = producto.Nombre,
                idCategoria = producto.CategoriasProductos.IdCategoria,
                nombreCategoria = producto.CategoriasProductos.Nombre,
                descripcion = producto.Descripcion,
                precio = producto.Precio,
                idEstado = producto.EstadosProductos.IdEstado,
                nombreEstado = producto.EstadosProductos.Nombre,
                imagen = producto.Imagen
            });
        }

        [HttpPost]
        public JsonResult editarProductos(FormCollection formCollection)
        {
            string rutaBase = "/Content/imagenes/";
            string carpeta = "Productos/";
            string ruta = "";

            HttpPostedFileBase imagen = Request.Files["imagenProductoEdicion"];
            

            ProductosRestaurante producto = new ProductosRestaurante();

            producto = db.ProductosRestaurante.Find(formCollection["idProducto"]);
            producto.IdCategoria = formCollection["categoriaProductoEdicion"];
            producto.Nombre = formCollection["nombreProductoEdicion"];
            producto.Descripcion = formCollection["descripcionProductoEdicion"];
            producto.Precio = Convert.ToDecimal(formCollection["precioProductoEdicion"]);
            producto.IdEstado = Convert.ToInt32(formCollection["estadoProductoEdicion"]);


            if (imagen.FileName!="" )
            {
                //Verificando que el nuevo archivo no exista
                ruta = obtenerRuta(imagen, rutaBase, carpeta, -1);

                if (operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre " + obtenerRuta(imagen, "", "", 0) + " ya se encuentra registrado" });
                }


                //eliminando imagen anterior
                operacionesImagenes(null, producto.Imagen, -1);

                //Asignando ruta nueva imagen
                producto.Imagen = obtenerRuta(imagen, rutaBase, carpeta, 1);

                //almacenando nueva imagen
                operacionesImagenes(imagen, ruta, 1);

            }

            //Registrando cambios

            db.Entry(producto).State = EntityState.Modified;
            db.SaveChanges();

            //Respuesta
            return Json(new { success = true, message = "Se actualizó con éxito" });

        }

        [HttpPost]
        public JsonResult eliminarProducto(string idProducto)
        {
            try
            {
                ProductosRestaurante productos = db.ProductosRestaurante.Find(idProducto);
                db.ProductosRestaurante.Remove(productos);
                db.SaveChanges();

                //eliminando imagen asociada
                operacionesImagenes(null, productos.Imagen, -1);

                return Json(new { success = true, message = "Se eliminó con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadProductos()
        {
            List<ProductosRestaurante> productos = new List<ProductosRestaurante>();
            productos = db.ProductosRestaurante.ToList();
            var productosClean = productos.Select(s => new {
                s.IdProducto,
                CategoriaNombre = s.CategoriasProductos.Nombre,
                s.Nombre,
                s.Precio,
                EstadoNombre = s.EstadosProductos.Nombre,
                s.Imagen
            });
            return Json(productosClean, JsonRequestBehavior.AllowGet);
        }
        //FIN METODOS PRODUCTOS

        //INICIO METODOS COMBOS
        public ActionResult Combos()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                ViewBag.IdEstados = new SelectList(db.EstadosProductos, "IdEstado", "Nombre");
                return View(db.Combos.Include(c => c.EstadosProductos).ToList());
            }
        }

        [HttpPost]
        public JsonResult Combos(FormCollection formCollection)
        {

            string rutaBase = "/Content/imagenes/";
            string carpeta = "Combos/";
            string ruta = "";
            HttpPostedFileBase imagen = Request.Files["imagenComboRegistro"];


            if (ModelState.IsValid)
            {
                //Verificando que archivo no exista 
                ruta = obtenerRuta(imagen, rutaBase, carpeta, -1);

                if (operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre " + obtenerRuta(imagen, "", "", 0) + " ya se encuentra registrado" });
                }
                //Guardando registro de Combo
                Combos combos = new Combos();
                combos.IdCombo = db.GeneradorIdOjetos("COM");
                combos.Nombre = formCollection["nombreCombo"];
                combos.Descripcion = formCollection["descripcionCombo"];
                combos.Precio = Convert.ToDecimal(formCollection["precioCombo"]);
                combos.IdEstado = Convert.ToInt32(formCollection["estadoComboRegistro"]);
                combos.Imagen = obtenerRuta(imagen, rutaBase, carpeta, 1);
                db.Combos.Add(combos);
                db.SaveChanges();

                //Guardando imagen
                imagen.SaveAs(ruta);

                //respuesta
                return Json(new { success = true, message = "Combo registrado con éxito" });

            }

            return Json(new { success = false, message = "El modelo no fue válido. Contacte a soporte" });

        }

        [HttpPost]
        public JsonResult obtenerCombo(string idCombo)
        {
            Combos combos = db.Combos.Find(idCombo);

            return Json(new
            {
                imagen = combos.Imagen,
                idEstado = combos.EstadosProductos.IdEstado,
                nombreEstado = combos.EstadosProductos.Nombre,
                nombre = combos.Nombre,
                precio = combos.Precio,
                descripcion = combos.Descripcion
            });
        }

        [HttpPost]
        public JsonResult editarCombos(FormCollection formCollection)
        {
            string rutaBase = "/Content/imagenes/";
            string carpeta = "Combos/";
            string ruta = "";

            HttpPostedFileBase imagen = Request.Files["imagenComboEdicion"];


            Combos combo = new Combos();

            combo = db.Combos.Find(formCollection["idCombo"]);
            combo.Nombre = formCollection["editarNombreCombo"];
            combo.Descripcion = formCollection["editarDescripcionCombo"];
            combo.Precio = Convert.ToDecimal(formCollection["editarPrecioCombo"]);
            combo.IdEstado = Convert.ToInt32(formCollection["estadoComboEdicion"]);

            if (imagen.FileName != "")
            {
                //Verificando que el nuevo archivo no exista
                ruta = obtenerRuta(imagen, rutaBase, carpeta, -1);

                if (operacionesImagenes(null, ruta, 0))
                {
                    //El archivo ya existe
                    return Json(new { success = false, message = "Un archivo con el nombre " + obtenerRuta(imagen, "", "", 0) + " ya se encuentra registrado" });
                }


                //eliminando imagen anterior
                operacionesImagenes(null, combo.Imagen, -1);

                //Asignando ruta nueva imagen
                combo.Imagen = obtenerRuta(imagen, rutaBase, carpeta, 1);

                //almacenando nueva imagen
                operacionesImagenes(imagen, ruta, 1);

            }

            //Registrando cambios

            db.Entry(combo).State = EntityState.Modified;
            db.SaveChanges();

            //Respuesta
            return Json(new { success = true, message = "Se actualizó con éxito" });

        }

        [HttpPost]
        public JsonResult eliminarCombo(string idCombo)
        {
            try
            {
                Combos combo = db.Combos.Find(idCombo);
                db.Combos.Remove(combo);
                db.SaveChanges();

                //eliminando imagen asociada
                operacionesImagenes(null, combo.Imagen, -1);

                return Json(new { success = true, message = "Se eliminó con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadCombos()
        {
            List<Combos> combos = new List<Combos>();
            combos = db.Combos.ToList();
            var combosClean = combos.Select(s => new {
                s.IdCombo,
                s.Nombre,
                s.Precio,
                nombreEstado = s.EstadosProductos.Nombre,
                s.Imagen
            });
            return Json(combosClean, JsonRequestBehavior.AllowGet);
        }


        //FIN METODOS COMBOS

        //INICIO METODOS GESTION COMBOS
        public ActionResult GestionarCombo(string idCombo)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                var model = new ModelGestionCombos();

                model.Combos = db.Combos.Find(idCombo);
                model.CombosDetalles = db.CombosDetalle.Where(a => a.IdCombo == idCombo).ToList();
                model.Productos = db.ProductosRestaurante.ToList();


                return View(model);
            }
        }

        [HttpPost]
        public JsonResult GestionarCombo([Bind(Include = "IdCombo,IdProducto,Cantidad")] CombosDetalle combosDetalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CombosDetalle.Add(combosDetalle);
                    db.SaveChanges();
                }
                return Json(new { success = true, message = "El producto se registró con éxito" });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult obtenerDetalleCombo(string idCombo)
        {
            List<CombosDetalle> CombosDetalle = new List<CombosDetalle>();
            CombosDetalle = db.CombosDetalle.Where(c=>c.IdCombo ==idCombo).ToList();
            var productosClean = CombosDetalle.Select(s => new {
                s.IdCombo,
                s.IdProducto,
                s.ProductosRestaurante.Imagen,
                s.ProductosRestaurante.Nombre,
                s.Cantidad
            });
            return Json(productosClean, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult eliminarProductoCombo(string idCombo, string idProducto)
        {
            try
            {
                CombosDetalle combo = db.CombosDetalle.Find(idCombo, idProducto);
                db.CombosDetalle.Remove(combo);
                db.SaveChanges();

                return Json(new { success = true, message = "Se eliminó con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }
        //FIN METODOS GESTION COMBOS

        //INICIO METODOS ETAPAS

        public ActionResult Etapas()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                return View(db.EtapasPedidos.ToList());
            }
        }

        [HttpPost]
        public void Etapas([Bind(Include = "IdEtapa,Nombre")] EtapasPedidos etapasPedidos)
        {
            if (ModelState.IsValid)
            {
                db.EtapasPedidos.Add(etapasPedidos);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void editarEtapas([Bind(Include = "IdEtapa,Nombre")] EtapasPedidos etapasPedidos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(etapasPedidos).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public JsonResult eliminarEtapa(int idEtapa)
        {
            try
            {
                EtapasPedidos etapasPedidos = db.EtapasPedidos.Find(idEtapa);
                db.EtapasPedidos.Remove(etapasPedidos);
                db.SaveChanges();

                return Json(new { success = true, message = "Etapa eliminada con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public JsonResult reloadEtapas()
        {
            List<EtapasPedidos> etapas = new List<EtapasPedidos>();
            etapas = db.EtapasPedidos.ToList();
            var etapasClean = etapas.Select(s => new {
                s.IdEtapa,
                s.Nombre
            });
            return Json(etapasClean, JsonRequestBehavior.AllowGet);
        }

        //FIN METODOS ETAPAS

        //INICIO METODOS HISTORIAL ORDENES

        public ActionResult HistorialOrdenes()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("inicio", "Cliente");
            }
            else
            {
                var model = new ModelHistorialOrdenes();

                ViewBag.IdEtapas = new SelectList(db.EtapasPedidos, "IdEtapa", "Nombre");
                model.pedidosClientes = db.PedidosClientes.ToList();

                return View(model);
            }
        }

        [HttpPost]
        public JsonResult obtenerClienteOrden( string idPedido)
        {
            PedidosClientes pedidos = db.PedidosClientes.Find(idPedido);

            return Json(new
            {
                nombre = (pedidos.CuentasClientes.Nombres+" "+ pedidos.CuentasClientes.Apellidos),
                direccion = pedidos.DireccionEntrega,
                telefono = pedidos.TelefonoContacto,
                correo = pedidos.CuentasClientes.Correo,
                fecha = pedidos.Fecha,
                estado = pedidos.TrackeoPedidosClientes.EtapasPedidos.Nombre,
                total = db.ObtenerMontoPedido(idPedido)
            });
        }

        [HttpPost]
        public JsonResult obtenerDetalleOrden(string idPedido)
        {
            var detalle = db.ObtenerDetallePedido(idPedido);
            
            return Json(detalle, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerHistorialOrden(string idPedido)
        {
            List<HistorialPedidos> historial = new List<HistorialPedidos>();
            historial = db.HistorialPedidos.Where(h=> h.IdPedido == idPedido).ToList() ;
            var historialClean = historial.Select(s => new {
                s.IdPedido,
                s.Fecha,
                s.EtapasPedidos.Nombre,
                s.UsuarioGrabacion
            });
            return Json(historialClean, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult reloadOrdenes()
        {
            List<PedidosClientes> ordenes = new List<PedidosClientes>();
            ordenes = db.PedidosClientes.ToList();
            var ordenesClean = ordenes.Select(s => new {
                s.IdPedido,
                nombreCliente = s.CuentasClientes.Nombres +" "+ s.CuentasClientes.Apellidos,
                s.Fecha,
                total= db.ObtenerMontoPedido(s.IdPedido),
                nombreEtapa = s.TrackeoPedidosClientes.EtapasPedidos.Nombre,
                s.TrackeoPedidosClientes.IdEtapa,
                s.IdCliente
            });
            return Json(ordenesClean, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editarEtapaPedido(string idPedido, int idEtapa)
        {
            
            try
            {
                PedidosClientes pedido = db.PedidosClientes.Find(idPedido);
                pedido.TrackeoPedidosClientes.IdEtapa = idEtapa;
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                string usuario = System.Web.HttpContext.Current.Session["id"] as String;
                registrarHistorial(idPedido,idEtapa,usuario);

                return Json(new { success = true, message = "Se editó con éxito" });
            }
            catch (DbUpdateException ex)
            { return Json(new { success = false, message = ex.Message }); }
        }
        //FIN METODOS HISTORIAL ORDENES


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

        [Authorize]
        private void registrarHistorial(string idPedido, int idEtapa, string usuario)
        {
            HistorialPedidos historial = new HistorialPedidos();

            historial.IdHistorial = 0;
            historial.IdPedido = idPedido;
            historial.IdEtapa = idEtapa;
            historial.UsuarioGrabacion = usuario;
            historial.Fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

            db.HistorialPedidos.Add(historial);
            db.SaveChanges();
        }

        // FIN METODOS AUXILIARES
    }
}