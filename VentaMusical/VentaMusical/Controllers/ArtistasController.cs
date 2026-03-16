using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models.ViewModels.Artistas;
using VentaMusical.Models;

namespace VentaMusical.Controllers
{
    public class ArtistasController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Artistas
        public ActionResult Index()
        {
            using (var db = new VentaMusicalEntities())
            {
                var artistas = db.Artistas.ToList();
                return View(artistas);
            }
        }



        // GET: Artistas/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new VentaMusicalEntities())
            {
                var artistaBD = db.Artistas.Find(id);
                if (artistaBD == null)
                    return HttpNotFound();

                var vm = new Artista
                {
                    CodigoArtista = artistaBD.CodigoArtista,
                    NombreArtistico = artistaBD.NombreArtistico,
                    FechaNacimiento = artistaBD.FechaNacimiento,
                    NombreReal = artistaBD.NombreReal,
                    Nacionalidad = artistaBD.Nacionalidad,
                    Foto = artistaBD.Foto,
                    LinkBiografia = artistaBD.LinkBiografia
                };

                return View(vm);
            }
        }


        // GET: Artistas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artistas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artista artista, HttpPostedFileBase archivoFoto)
        {
            if (ModelState.IsValid)
            {

                using (var db = new VentaMusicalEntities())
                {
                    var nuevoArtista = new Artistas
                    {
                        NombreArtistico = artista.NombreArtistico,
                        FechaNacimiento = artista.FechaNacimiento,
                        NombreReal = artista.NombreReal,
                        Nacionalidad = artista.Nacionalidad,
                        Foto = artista.Foto,
                        LinkBiografia = artista.LinkBiografia
                    };

                    db.Artistas.Add(nuevoArtista);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(artista);
        }



        // GET: Artistas/Edit/5
        public ActionResult Edit(int id)
        {
            var artista = db.Artistas.Find(id);
            if (artista == null)
            {
                return HttpNotFound();
            }
            return View(artista);
        }



        // POST: Artistas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artistas artista, HttpPostedFileBase archivoFoto)
        {
            if (ModelState.IsValid)
            {
                var artistaBD = db.Artistas.Find(artista.CodigoArtista);

                if (artistaBD == null)
                {
                    return HttpNotFound();
                }

                artistaBD.NombreArtistico = artista.NombreArtistico;
                artistaBD.NombreReal = artista.NombreReal;
                artistaBD.FechaNacimiento = artista.FechaNacimiento;
                artistaBD.Nacionalidad = artista.Nacionalidad;
                artistaBD.LinkBiografia = artista.LinkBiografia;
                artistaBD.Foto = artista.Foto;

                db.Entry(artistaBD).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(artista);
        }

        // GET: Artistas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var artista = db.Artistas.Find(id);
            if (artista == null)
            {
                return HttpNotFound();
            }

            // Convertir a ViewModel si estás usando uno
            var artistaVM = new VentaMusical.Models.ViewModels.Artistas.Artista
            {
                CodigoArtista = artista.CodigoArtista,
                NombreArtistico = artista.NombreArtistico,
                FechaNacimiento = artista.FechaNacimiento,
                NombreReal = artista.NombreReal,
                Nacionalidad = artista.Nacionalidad,
                Foto = artista.Foto,
                LinkBiografia = artista.LinkBiografia
            };

            return View(artistaVM);
        }

        // POST: Artistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var artista = db.Artistas.Find(id);
            if (artista != null)
            {
                db.Artistas.Remove(artista);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
