using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Usuarios;

namespace VentaMusical.Controllers
{
    public class UsuariosController : Controller
    {

        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Usuarios
        public ActionResult Index(string busquedaCedula)
        {
            try
            {
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var usuariosFiltrados = db.Usuarios.AsQueryable();

                    if (!string.IsNullOrEmpty(busquedaCedula))
                    {
                        usuariosFiltrados = usuariosFiltrados.Where(c => c.NumeroIdentificacion.Contains(busquedaCedula));
                    }

                    var viewModel = usuariosFiltrados.Select(s => new Models.ViewModels.Usuarios.ListarUsuarios
                    {
                        ID = s.ID,
                        NumeroIdentificacion = s.NumeroIdentificacion,
                        Nombre = s.Nombre,
                        Apellido = s.Apellido,
                        Genero = s.Genero,
                        Correo = s.CorreoElectronico,
                        TipoTarjeta = s.TipoTarjeta,
                        DineroDisponible = s.DineroDisponible,
                        NumeroTarjeta = s.NumeroTarjeta,
                        Contrasena = s.Contrasena,
                        Perfil = s.Perfil
                    }).ToList();

                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return View(new List<Models.ViewModels.Usuarios.ListarUsuarios>());
            }
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Usuario usuario = new Usuario();
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var datosUsuario = db.Usuarios.FirstOrDefault(s => s.ID == id);
                    if (datosUsuario != null)
                    {
                        usuario.IDUsuario = datosUsuario.ID;
                        usuario.NumeroIdentificacion = datosUsuario.NumeroIdentificacion;
                        usuario.Nombre = datosUsuario.Nombre;
                        usuario.Apellido = datosUsuario.Apellido;
                        usuario.Genero = datosUsuario.Genero;
                        usuario.Correo = datosUsuario.CorreoElectronico;
                        usuario.TipoTarjeta = datosUsuario.TipoTarjeta;
                        usuario.DineroDisponible = datosUsuario.DineroDisponible;
                        usuario.NumeroTarjeta = datosUsuario.NumeroTarjeta;
                        usuario.Contrasena = datosUsuario.Contrasena;
                        usuario.Perfil = datosUsuario.Perfil;
                    }

                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Usuarios", "Details"));
            }
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(CrearUsuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    Usuarios nuevoUsuario = new Usuarios()
                    {
                        NumeroIdentificacion = usuario.NumeroIdentificacion,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Genero = usuario.Genero,
                        CorreoElectronico = usuario.Correo,
                        TipoTarjeta = usuario.TipoTarjeta,
                        DineroDisponible = usuario.DineroDisponible,
                        NumeroTarjeta = usuario.NumeroTarjeta,
                        Contrasena = usuario.Contrasena,
                        Perfil = usuario.Perfil,
                    };

                    db.Usuarios.Add(nuevoUsuario);
                    db.SaveChanges();

                    ViewBag.MensajeProceso = "Usuario creado exitosamente";
                    ViewBag.ValorMensaje = 1;
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al crear el usuario " + ex.Message;
                ViewBag.ValorMensaje = 0;
                return View(usuario);
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                EditarUsuario usuario = new EditarUsuario();
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var datosUsuario = db.Usuarios.FirstOrDefault(s => s.ID == id);
                    if (datosUsuario != null)
                    {
                        usuario.IDUsuario = datosUsuario.ID;
                        usuario.NumeroIdentificacion = datosUsuario.NumeroIdentificacion;
                        usuario.Nombre = datosUsuario.Nombre;
                        usuario.Apellido = datosUsuario.Apellido;
                        usuario.Genero = datosUsuario.Genero;
                        usuario.Correo = datosUsuario.CorreoElectronico;
                        usuario.TipoTarjeta = datosUsuario.TipoTarjeta;
                        usuario.DineroDisponible = datosUsuario.DineroDisponible;
                        usuario.NumeroTarjeta = datosUsuario.NumeroTarjeta;
                        usuario.Contrasena = datosUsuario.Contrasena;
                        usuario.Perfil = datosUsuario.Perfil;
                    }

                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Usuarios", "Edit"));
            }
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit(EditarUsuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var usuarioActual = db.Usuarios.Find(usuario.IDUsuario);
                    if (usuarioActual != null)
                    {
                        usuarioActual.NumeroIdentificacion = usuario.NumeroIdentificacion;
                        usuarioActual.Nombre = usuario.Nombre;
                        usuarioActual.Apellido = usuario.Apellido;
                        usuarioActual.Genero = usuario.Genero;
                        usuarioActual.CorreoElectronico = usuario.Correo;
                        usuarioActual.TipoTarjeta = usuario.TipoTarjeta;
                        usuarioActual.DineroDisponible = usuario.DineroDisponible;
                        usuarioActual.NumeroTarjeta = usuario.NumeroTarjeta;
                        usuarioActual.Contrasena = usuario.Contrasena;
                        usuarioActual.Perfil = usuario.Perfil;
                    }

                    db.Entry(usuarioActual).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.MensajeProceso = "Usuario editado exitosamente";
                    ViewBag.ValorMensaje = 1;

                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al editar el usuario " + ex.Message;
                ViewBag.ValorMensaje = 0;
                return View(usuario);
            }
        }

        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Find(id);

            if (usuario == null)
                return HttpNotFound();

            var modelo = new Usuario
            {
                IDUsuario = usuario.ID,
                NumeroIdentificacion = usuario.NumeroIdentificacion,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Genero = usuario.Genero,
                Correo = usuario.CorreoElectronico,
                TipoTarjeta = usuario.TipoTarjeta,
                DineroDisponible = usuario.DineroDisponible,
                NumeroTarjeta = usuario.NumeroTarjeta,
                Contrasena = usuario.Contrasena,
                Perfil = usuario.Perfil,
            };

            return View(modelo);
        }


        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            using (var db = new VentaMusicalEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.CorreoElectronico == user.Correo && u.Contrasena == user.Contrasena);

                if (usuario != null)
                {
                    // Obtenemos el nombre del usuario
                    Session["Usuario"] = usuario.Nombre;

                    // Cedula del usuario
                    Session["UsuarioId"] = usuario.ID;
                    
                    // Rol
                    Session["Rol"] = usuario.Perfil; 

                    return RedirectToAction("Dashboard", "Home");
                }

                TempData["LoginError"] = "Credenciales incorrectas.";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Elimina todos los datos de sesión
            return RedirectToAction("Index", "Home"); // Vuelve al inicio o login
        }


    }
}
