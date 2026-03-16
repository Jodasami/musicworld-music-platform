using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Canciones;

namespace VentaMusical.Controllers
{
    public class CancionesController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Canciones
        public ActionResult Index(string busquedaNombre)
        {
            try
            {
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var cancionesFiltradas = db.Canciones.AsQueryable().Include("Albumes.Artistas");

                    if (!string.IsNullOrEmpty(busquedaNombre))
                    {
                        cancionesFiltradas = cancionesFiltradas.Where(c => c.NombreCancion.Contains(busquedaNombre));
                    }

                    var viewModel = cancionesFiltradas
                        .Select(s => new Models.ViewModels.Canciones.ListarCanciones
                    {
                        CodigoCancion = s.CodigoCancion,
                        CodigoGenero = s.CodigoGenero,
                        CodigoAlbum = s.CodigoAlbum,
                        NombreCancion = s.NombreCancion,
                        LinkVideo = s.LinkVideo,
                        Precio = s.Precio,
                        CantidadDisponible = s.CantidadDisponible,
                        NombreGenero = s.Generos.Descripcion,
                        NombreAlbum = s.Albumes.NombreAlbum,
                        ImagenAlbum = s.Albumes.Imagen,
                        }).ToList();

                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return View(new List<Models.ViewModels.Canciones.ListarCanciones>());
            }
        }


        public ActionResult IndexUser(string busquedaNombre)
        {
            try
            {
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var cancionesFiltradas = db.Canciones.AsQueryable();

                    if (!string.IsNullOrEmpty(busquedaNombre))
                    {
                        cancionesFiltradas = cancionesFiltradas.Where(c => c.NombreCancion.Contains(busquedaNombre));
                    }

                    var viewModel = cancionesFiltradas
                        .Select(s => new Models.ViewModels.Canciones.ListarCanciones
                        {
                            CodigoCancion = s.CodigoCancion,
                            CodigoGenero = s.CodigoGenero,
                            CodigoAlbum = s.CodigoAlbum,
                            NombreCancion = s.NombreCancion,
                            LinkVideo = s.LinkVideo,
                            Precio = s.Precio,
                            CantidadDisponible = s.CantidadDisponible,
                            NombreGenero = s.Generos.Descripcion,
                            NombreAlbum = s.Albumes.NombreAlbum

                        }).ToList();

                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return View(new List<Models.ViewModels.Canciones.ListarCanciones>());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var datos = db.Canciones
                        .Include("Albumes.Artistas")
                        .Include("Generos")
                        .FirstOrDefault(c => c.CodigoCancion == id);

                    if (datos == null)
                        return HttpNotFound();

                    var model = new DetallesCancion
                    {
                        // Datos de la canción
                        CodigoCancion = datos.CodigoCancion,
                        CodigoGenero = datos.CodigoGenero,
                        CodigoAlbum = datos.CodigoAlbum,
                        NombreCancion = datos.NombreCancion,
                        LinkVideo = datos.LinkVideo,
                        Precio = datos.Precio,
                        CantidadDisponible = datos.CantidadDisponible,

                        // Datos del género
                        NombreGenero = datos.Generos?.Descripcion,

                        // Datos del álbum
                        NombreAlbum = datos.Albumes?.NombreAlbum,
                        ImagenAlbum = datos.Albumes?.Imagen,
                        Anno= datos.Albumes.AnoLanzamiento,

                        // Datos del artista
                        CodigoArtista = datos.Albumes.Artistas?.CodigoArtista ?? 0,
                        NombreArtistico = datos.Albumes.Artistas?.NombreArtistico,
                        FechaNacimiento = datos.Albumes.Artistas?.FechaNacimiento,
                        NombreReal = datos.Albumes.Artistas?.NombreReal,
                        Nacionalidad = datos.Albumes.Artistas?.Nacionalidad,
                        Foto = datos.Albumes.Artistas?.Foto,
                        LinkBiografia = datos.Albumes.Artistas?.LinkBiografia
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Canciones", "Details"));
            }
        }

        public ActionResult DetailsUser(int id)
        {
            try
            {
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var datos = db.Canciones
                        .Include("Albumes.Artistas")
                        .Include("Generos")
                        .FirstOrDefault(c => c.CodigoCancion == id);

                    if (datos == null)
                        return HttpNotFound();

                    var model = new DetallesCancion
                    {
                        // Datos de la canción
                        CodigoCancion = datos.CodigoCancion,
                        CodigoGenero = datos.CodigoGenero,
                        CodigoAlbum = datos.CodigoAlbum,
                        NombreCancion = datos.NombreCancion,
                        LinkVideo = datos.LinkVideo,
                        Precio = datos.Precio,
                        CantidadDisponible = datos.CantidadDisponible,

                        // Datos del género
                        NombreGenero = datos.Generos?.Descripcion,

                        // Datos del álbum
                        NombreAlbum = datos.Albumes?.NombreAlbum,
                        ImagenAlbum = datos.Albumes?.Imagen,
                        Anno = datos.Albumes.AnoLanzamiento,

                        // Datos del artista
                        CodigoArtista = datos.Albumes.Artistas?.CodigoArtista ?? 0,
                        NombreArtistico = datos.Albumes.Artistas?.NombreArtistico,
                        FechaNacimiento = datos.Albumes.Artistas?.FechaNacimiento,
                        NombreReal = datos.Albumes.Artistas?.NombreReal,
                        Nacionalidad = datos.Albumes.Artistas?.Nacionalidad,
                        Foto = datos.Albumes.Artistas?.Foto,
                        LinkBiografia = datos.Albumes.Artistas?.LinkBiografia
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Canciones", "Details"));
            }
        }


        // GET: Canciones/Create
        public ActionResult Create()
        {
            using (var db = new VentaMusicalEntities())
            {
                var generos = db.Generos.ToList();
                var albumes = db.Albumes.ToList();

                var model = new CrearCancion
                {
                    GenerosDisponibles = generos.Select(g => new SelectListItem
                    {
                        Value = g.CodigoGenero.ToString(),
                        Text = g.Descripcion
                    }).ToList(),

                    AlbumesDisponibles = albumes.Select(a => new SelectListItem
                    {
                        Value = a.CodigoAlbum.ToString(),
                        Text = a.NombreAlbum + " (" + a.CodigoAlbum + ")"
                    }).ToList()
                };

                return View(model);
            }
        }
        

        // POST: Canciones/Create
        [HttpPost]
        public ActionResult Create(CrearCancion cancion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PoblarListas(cancion);
                    return View(cancion);
                }
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    Canciones nuevaCancion = new Canciones()
                    {
                        CodigoGenero = cancion.CodigoGenero,
                        CodigoAlbum = cancion.CodigoAlbum,
                        NombreCancion = cancion.NombreCancion,
                        LinkVideo = cancion.LinkVideo,
                        Precio = cancion.Precio,
                        CantidadDisponible = cancion.CantidadDisponible,
                    };

                    db.Canciones.Add(nuevaCancion);
                    db.SaveChanges();

                    ViewBag.MensajeProceso = "Canción creado exitosamente";
                    ViewBag.ValorMensaje = 1;

                    PoblarListas(cancion);
                    return View(cancion);

                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al crear la canción " + ex.Message;
                ViewBag.ValorMensaje = 0;

                PoblarListas(cancion);
                return View(cancion);
            }
        }

        // GET: Canciones/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                EditarCancion cancion = new EditarCancion();
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var datosCancion = db.Canciones.FirstOrDefault(s => s.CodigoCancion == id);
                    if (datosCancion != null)
                    {
                        cancion.CodigoCancion = datosCancion.CodigoCancion;
                        cancion.CodigoGenero = datosCancion.CodigoGenero;
                        cancion.CodigoAlbum = datosCancion.CodigoAlbum;
                        cancion.NombreCancion = datosCancion.NombreCancion;
                        cancion.LinkVideo = datosCancion.LinkVideo;
                        cancion.Precio = datosCancion.Precio;
                        cancion.CantidadDisponible = datosCancion.CantidadDisponible;

                        PoblarListas(cancion);
                    }

                    return View(cancion);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Canciones", "Edit"));
            }
        }

        // POST: Canciones/Edit/5
        [HttpPost]
        public ActionResult Edit(EditarCancion cancion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cancion);
                }
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    var cantidadActual = db.Canciones.Find(cancion.CodigoCancion);
                    if (cantidadActual != null)
                    {
                        cantidadActual.CodigoGenero = cancion.CodigoGenero;
                        cantidadActual.CodigoAlbum = cancion.CodigoAlbum;
                        cantidadActual.NombreCancion = cancion.NombreCancion;
                        cantidadActual.LinkVideo = cancion.LinkVideo;
                        cantidadActual.Precio = cancion.Precio;
                        cantidadActual.CantidadDisponible = cancion.CantidadDisponible;
                    }

                    db.Entry(cantidadActual).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.MensajeProceso = "Canción editada exitosamente";
                    ViewBag.ValorMensaje = 1;

                    PoblarListas(cancion);
                    return View(cancion);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al editar la canción " + ex.Message;
                ViewBag.ValorMensaje = 0;

                PoblarListas(cancion);
                return View(cancion);
            }
        }

        public ActionResult Delete(int id)
        {
            var cancion = db.Canciones.Find(id);

            if (cancion == null)
                return HttpNotFound();

            var modelo = new Cancion
            {
                CodigoCancion = cancion.CodigoCancion,
                CodigoGenero = cancion.CodigoGenero,
                CodigoAlbum = cancion.CodigoAlbum,
                NombreCancion = cancion.NombreCancion,
                LinkVideo = cancion.LinkVideo,
                Precio = cancion.Precio,
                CantidadDisponible = cancion.CantidadDisponible,
            };

            return View(modelo);
        }


        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cancion = db.Canciones.Find(id);
            db.Canciones.Remove(cancion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        private void PoblarListas(CrearCancion model)
        {
            using (var db = new VentaMusicalEntities())
            {
                var generos = db.Generos.ToList();
                var albumes = db.Albumes.ToList();

                model.GenerosDisponibles = generos.Select(g => new SelectListItem
                {
                    Value = g.CodigoGenero.ToString(),
                    Text = g.Descripcion
                }).ToList();

                model.AlbumesDisponibles = albumes.Select(a => new SelectListItem
                {
                    Value = a.CodigoAlbum.ToString(),
                    Text = a.NombreAlbum + " (" + a.CodigoAlbum + ")"
                }).ToList();
            }
        }


    }
}
