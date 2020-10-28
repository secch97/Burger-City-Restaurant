using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using RestauranteWeb.Models;
using System.Reflection.Emit;
using System.Data.Entity.Validation;
using System.Web.Security;

namespace RestauranteWeb.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Inicio()
        {
            //Insertar aca código para mostrar en página principal.
            return View();
        }

        [HttpGet]
        public ActionResult Registrarme()
        {
            return View();
        }

        //Obtener datos de el formulario de registro
        [HttpPost]
        public ActionResult Registrarme([Bind(Exclude = "Validado,IdCliente,Fecha")] CuentasClientes cuentasClientes)
        {
            bool Status = false;
            string Message = "";
            //validar en el modelo
            if (ModelState.IsValid)
            {
                
                #region //Validar si existe el correo
                    
                var existeEmail = Correo_existe(cuentasClientes.Correo);
                if (existeEmail)
                {
                    ModelState.AddModelError("Correo_Existe","Este correo existe");
                    return View(cuentasClientes);
                }
                #endregion

                #region contraseña hash
                cuentasClientes.Clave = Crypto.Hash(cuentasClientes.ConfimarClave);
                cuentasClientes.ConfimarClave = Crypto.Hash(cuentasClientes.ConfimarClave);
                #endregion

                cuentasClientes.Validado = false;

                int asar = new Random().Next(0, 100000); ;
                string codigo = "TC" + asar.ToString();
                var Existe_id = Id_extiste(codigo);
                if (Existe_id)
                {
                    
                }
                else
                {
                    do
                    {
                         asar = new Random().Next(0, 100000); ;
                         codigo = "TC" + asar.ToString();
                         Existe_id = Id_extiste(codigo);
                    } while (Existe_id==true);
                    
                }
                cuentasClientes.IdCliente = codigo;

                DateTime now = DateTime.Now;
                cuentasClientes.Fecha = now;


                #region guardar en base de datos 
                using (ProyectoASP_RestauranteEntities pr = new ProyectoASP_RestauranteEntities())
                {
                    pr.CuentasClientes.Add(cuentasClientes);
                    
                        pr.SaveChanges();

                        //enviar email de validacion;
                        Correo_Verificaion(cuentasClientes.Correo, cuentasClientes.IdCliente.ToString());
                        Message = "Registro Exitoso. Revise su correo para validar su cuenta." + cuentasClientes.Correo;
                        Status = true;
                   
                    

                }
                #endregion

                
            }
            else
            {
                Message = "Valor incorrecto";
            }


            return View(cuentasClientes);
        }

        //verificar cuenta
        [HttpGet]
        public ActionResult VerificarCuenta(string id)
        {
            bool Status = false;
            using (ProyectoASP_RestauranteEntities pr = new ProyectoASP_RestauranteEntities())
            {
                pr.Configuration.ValidateOnSaveEnabled = false;// esta linea confirma que las contraseñas no coinsidas
                var v = pr.CuentasClientes.Where(a => a.IdCliente == id).FirstOrDefault();
                if (v!=null)
                {
                    v.Validado = true;
                    pr.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "No se pudo validar";
                }

            }
            ViewBag.Status = Status;
            return View();
        }


        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login,string returnUrl)
        {
            string message = "";
            using(ProyectoASP_RestauranteEntities pr = new ProyectoASP_RestauranteEntities())
            {
                var v = pr.CuentasClientes.Where(a => a.Correo == login.Correo).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Clave), v.Clave) == 0)
                    {
                        int timeout = login.recordarme ? 52560 : 20; //525600 es una hora
                        var ticket = new FormsAuthenticationTicket(login.Correo, login.recordarme, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("inicio", "Cliente");
                        }
                    }
                    else
                    {
                        message = "Credenciales incorrectas";
                    }

                }
                else
                {
                    var emp = pr.CuentasEmpleados.Where(a => a.Usuario == login.Correo).FirstOrDefault();
                    if (emp != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Clave), emp.Clave) == 0)
                        {
                            int timeout = login.recordarme ? 52560 : 20; //525600 es una hora
                            var ticket = new FormsAuthenticationTicket(login.Correo, login.recordarme, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                //hay que hacer la comparacion si es delivery , admin o restaurante
                                return RedirectToAction("Index", "Administrador");
                            }
                        }
                        else
                        {
                            message = "Credenciales incorrectas";
                        }
                    }
                    else
                    {
                        message = "Este correo no esta registrado";
                    }
                }
            }

            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("inicio","Cliente");
        }

        



        [NonAction]
        public bool Correo_existe (string Correo)
        {
            using(ProyectoASP_RestauranteEntities pre = new ProyectoASP_RestauranteEntities())
            {
                var v = pre.CuentasClientes.Where(a => a.Correo == Correo).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool Id_extiste(string id)
        {
            using (ProyectoASP_RestauranteEntities pre = new ProyectoASP_RestauranteEntities())
            {
                var v = pre.CuentasClientes.Where(a => a.IdCliente == id).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void Correo_Verificaion(string email, string id)
        {
            var verificarurl = "/Cliente/VerificarCuenta/" + id;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verificarurl);

            var fromEmail = new MailAddress("isidroprueba005@gmail.com", "Burger");
            var toEmail = new MailAddress(email);
            var fromEmailContra = "pruebas-8";
            string subject = "Tu cuenta fue creada exitosamente";
            string body = "<br><br> Estamso felices que quieras esta en la familia de Burger" +
                "Creado exitosamente. Por favor Ingrese a el siguiente link para varificar su cuenta" +
                "<br><br><a href='"+link+"'>"+link+"</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailContra)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            smtp.Send(message);
        }

    }
}